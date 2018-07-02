using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.IO;
using System.Drawing;


namespace BLL
{
    public class MedicalHistoryBusinessLayer
    {
        private CarePointEntities DBEntities;

        public MedicalHistoryBusinessLayer()
        {
            DBEntities = new CarePointEntities();
        }

        /// <summary>
        /// save an attachment to database 
        /// </summary>
        public void SaveAttachment(Attachment attachment)
        {
            DBEntities.Attachments.Add(attachment);
            DBEntities.SaveChanges();
        }

        /// <summary>
        /// get attachment types from databse such as (Radiology, lab investigation, ...)
        /// </summary>
        public ICollection<AttachmentType> GetAttachmentTypes()
        {
            return DBEntities.AttachmentTypes.ToList();
        }

        /// <summary>
        /// save prescription as an history record to patient 
        /// and as an prescription image attachment
        /// </summary>
        /// <param name="historyRecord">
        /// record that contains symptoms, diseases, medicines and remarks that was writen to patient
        /// </param>
        /// <param name="patientMedicines">written medicines names</param>
        /// <param name="dosesDescription">written medicines doses</param>
        /// <param name="medicinesAlternatives">written medicines alternatives</param>
        /// <param name="savingPath">
        /// path that will be saved as attribute in attachment for prscription image
        /// </param>
        /// <param name="notifyPrognosis">user ID and diseases that marked as genetic by doctor</param>
        /// <returns>bitmap prescription image to be saved to server then downloaded</returns>
        public Bitmap SavePrescription(HistoryRecord historyRecord, string[] patientMedicines,
            string[] dosesDescription, List<List<string>> medicinesAlternatives, string savingPath,Action<long,string> notifyPrognosis = null)
        {
            PrescriptionCanvas canvas = new PrescriptionCanvas();
            Bitmap bitmap = null;
            string medicineName;
            int prescriptionTypeID = 3;

            // get IDs of patient medicines names then assign doses to history record
            for (int i = 0; i < patientMedicines.Length; i++)
            {
                medicineName = patientMedicines[i];
                if (!medicineName.Equals(""))
                {
                    Medicine medicine = DBEntities.Medicines.FirstOrDefault(m => m.Name == medicineName);
                    if (medicine == null)
                    {
                        return null;
                    }
                        
                    historyRecord.Doses.Add(new Dose
                    {
                        MedicineID = medicine.ID,
                        Description = dosesDescription[i]
                    });
                }
            }

            // save history record to database
            historyRecord.IsRead = false;
            historyRecord = DBEntities.HistoryRecords.Add(historyRecord);
            
            // get whole object of this history record
            historyRecord.MedicalPlace = DBEntities.MedicalPlaces.Single(medicalPlace =>
                                        medicalPlace.ID == historyRecord.MedicalPlaceID);
            historyRecord.Citizen = DBEntities.Citizens.Single(citizen =>
                                        citizen.Id == historyRecord.CitizenID);
            historyRecord.Specialist = DBEntities.Citizens.OfType<Specialist>().Single(specialist =>
                                        specialist.Id == historyRecord.SpecialistID);
            
            // create prescription only if doctor wrote drugs to patient
            if(patientMedicines[0] != "")
            {
                // add prescription as an attachment
                Attachment attachment = new Attachment
                {
                    TypeID = prescriptionTypeID,
                    Date = historyRecord.Date,
                    SpecialistID = historyRecord.SpecialistID,
                    CitizenID = historyRecord.CitizenID,
                    FilePath = savingPath,
                    FileName = Path.GetFileName(savingPath),
                    IsRead = false
                };

                // save attachment to database
                SaveAttachment(attachment);

                // Draw prescription as image
                bitmap = canvas.Draw(historyRecord, patientMedicines,
                medicinesAlternatives,dosesDescription);
            }
            NotifyForGeneticDiseases(historyRecord.Citizen,historyRecord,0,true,notifyPrognosis);
            DBEntities.SaveChanges();
            return bitmap;
        }

        /// <summary>
        /// Notify Citizen for potential disease
        /// </summary>
        /// <param name="citizen">Citize</param>
        /// <param name="record">History Record</param>
        /// <param name="level">Level</param>
        /// <param name="includeSiblings">Notify Siblings of current Citizen</param>
        /// <param name="notifyPrognosis">a delegate for notification function</param>
        private void NotifyForGeneticDiseases(Citizen citizen,HistoryRecord record, int level,bool includeSiblings, Action<long, string> notifyPrognosis = null)
        {
            if (level > citizen.PrognosisMaxLevel)
                return;
            if (record.CitizenID != citizen.Id)
            {
                foreach (var disease in record.Diseases)
                {
                    if (disease.IsGenetic??false)
                    {
                        PotentialDisease potentialDisease = new PotentialDisease()
                        {
                            CitizenID = citizen.Id,
                            DiseaseID = disease.ID,
                            TimeStamp = DateTime.Now,
                            Level = level
                        };
                        DBEntities.PotentialDiseases.Add(potentialDisease);
                        notifyPrognosis(citizen.Id, disease.Name);
                    }
                }
            }
            if (includeSiblings)
            {
                var siblings = citizen.Relatives.Where(r => r.RelationTypeID == 2).Select(r => r.Citizen);
                foreach(Citizen s in siblings)
                {
                    NotifyForGeneticDiseases(s, record, level, false, notifyPrognosis);
                }
            }
            var children = citizen.Relatives.Where(r => r.RelationTypeID == 1).Select(r => r.Citizen);
            foreach(Citizen c in children)
            {
                NotifyForGeneticDiseases(c, record, level + 1, false,notifyPrognosis);
            }
        }

    }
}
