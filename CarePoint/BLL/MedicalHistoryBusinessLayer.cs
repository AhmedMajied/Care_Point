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
            string[] dosesDescription, List<List<string>> medicinesAlternatives, string savingPath)
        {
            Canvas canvas = new Canvas();
            string medicineName;
            int prescriptionTypeID = 3;

            // get IDs of patient medicines names then assign doses to history record
            for (int i = 0; i < patientMedicines.Length; i++)
            {
                medicineName = patientMedicines[i];
                if (!medicineName.Equals("") && !medicineName.Equals(" "))
                {
                    Medicine medicine = DBEntities.Medicines.Single(m => m.Name == medicineName);
                    historyRecord.Doses.Add(new Dose
                    {
                        MedicineID = medicine.ID,
                        Description = dosesDescription[i]
                    });
                }
            }

            // save history record to database
            historyRecord = DBEntities.HistoryRecords.Add(historyRecord);
            DBEntities.SaveChanges();
<<<<<<< HEAD

            // get whole object of this history record
            historyRecord.MedicalPlace = DBEntities.MedicalPlaces.Single(medicalPlace =>
                                        medicalPlace.ID == historyRecord.MedicalPlaceID);
            historyRecord.Citizen = DBEntities.Citizens.Single(citizen =>
                                        citizen.Id == historyRecord.CitizenID);
            historyRecord.Specialist = DBEntities.Citizens.OfType<Specialist>().Single(specialist =>
                                        specialist.Id == historyRecord.SpecialistID);
            
            // add prescription as an attachment
            Attachment attachment = new Attachment
            {
                TypeID = prescriptionTypeID,
                Date = historyRecord.Date,
                SpecialistID = historyRecord.SpecialistID,
                CitizenID = historyRecord.CitizenID,
                FilePath = savingPath,
                FileName = Path.GetFileName(savingPath)
            };
=======
        }
>>>>>>> master

            // save attachment to database
            SaveAttachment(attachment);

            // Draw prescription as image
            Bitmap bitmap = canvas.convertTextToImage(historyRecord, patientMedicines,
                medicinesAlternatives);

            return bitmap;
        }

    }
}
