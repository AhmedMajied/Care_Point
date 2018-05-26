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

        public void SaveAttachment(Attachment attachment)
        {
            DBEntities.Attachments.Add(attachment);
            DBEntities.SaveChanges();
        }

        public ICollection<AttachmentType> GetAttachmentTypes()
        {
            return DBEntities.AttachmentTypes.ToList();
        }

        public Bitmap SavePrescription(HistoryRecord historyRecord, string[] patientMedicines,
            string[] dosesDescription, List<List<string>> medicinesAlternatives, string savingPath,Action<long,string> notifyPrognosis = null)
        {
            Canvas canvas = new Canvas();
            Bitmap bitmap = null;
            string medicineName;
            int prescriptionTypeID = 3;

            // get IDs of patient medicines names then assign doses to history record
            for (int i = 0; i < patientMedicines.Length; i++)
            {
                medicineName = patientMedicines[i];
                if (!medicineName.Equals("") && !medicineName.Equals(" "))
                {
                    Medicine medicine = DBEntities.Medicines.FirstOrDefault(m => m.Name == medicineName);
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
                bitmap = canvas.drawText(historyRecord, patientMedicines,
                medicinesAlternatives);
            }
            NotifyForGeneticDiseases(historyRecord.Citizen,historyRecord,0,true,notifyPrognosis);
            DBEntities.SaveChanges();
            return bitmap;
        }

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
