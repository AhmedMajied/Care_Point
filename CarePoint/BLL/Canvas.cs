using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Web;
using DAL;

namespace BLL
{
    public class Canvas
    {
        private string newLine = "\n\n\n";
        private int pageWidth, pageHeight, padding = 10;
        private int imageWidth = 100, imageHeight = 100;
        private Font regular, bold;
        private Bitmap bitmap;
        private Graphics graphics;
        private StringFormat alignmentCenter, alignmentRight;

        public Canvas()
        {
            regular = new Font("Helvetica", 24, FontStyle.Regular, GraphicsUnit.Pixel);
            bold = new Font("Helvetica", 28, FontStyle.Bold, GraphicsUnit.Pixel);

            bitmap = new Bitmap(1, 1);
            graphics = Graphics.FromImage(bitmap);

            // alignment positions
            alignmentCenter = new StringFormat();
            alignmentCenter.Alignment = StringAlignment.Center;

            alignmentRight = new StringFormat();
            alignmentRight.Alignment = StringAlignment.Far;
        }

        public Bitmap convertTextToImage(HistoryRecord historyRecord, string[] medicines,
            List<List<string>> medicinesAlternatives)
        {

            // decide page structure to know the sutable legth of it 
            string pageStructure = "\n\n\nName \n Patient name \n Medicines " + "\n\n"
                    + " Doctor \n Doctor name \n\n\n\n Address \n medicalPlaceAddress";

            for (int i = 0; i < medicinesAlternatives.Count; i++)
            {
                for(int c=0;c < medicinesAlternatives[i].Count -1;c++)
                    pageStructure += "\n";
                pageStructure += "\n";
            }

            pageWidth = 700;
            pageHeight = (int)graphics.MeasureString(pageStructure, bold).Height + imageHeight;

            bitmap = new Bitmap(bitmap, new Size(pageWidth, pageHeight));
            graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.White); // white background
            // Drawing in high quality
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.InterpolationMode = InterpolationMode.High;

            // Drawing
            DrawPrescriptionHeader(historyRecord.MedicalPlace.Name, historyRecord.MedicalPlace.Photo);
            DrawPrescriptionBody(historyRecord.Citizen.Name, historyRecord.Specialist.Name,
                historyRecord.Date, medicines,medicinesAlternatives);
            DrawPrescriptionFooter(historyRecord.MedicalPlace.Address, historyRecord.MedicalPlace.Phone);

            graphics.Flush();
            graphics.Dispose();

            return bitmap;
        }

        private void DrawPrescriptionHeader(string medicalPlaceName, byte[] medicalPlaceImg)
        {
            int leftPadding = 50;
            int upperPadding = 10;

            if (medicalPlaceImg != null)
            {
                Image image = (Bitmap)((new ImageConverter()).ConvertFrom(medicalPlaceImg));
                graphics.DrawImage(image, leftPadding, upperPadding, leftPadding + imageWidth, upperPadding + imageHeight);
                graphics.DrawString(medicalPlaceName, new Font("Helvetica", 40, FontStyle.Bold, GraphicsUnit.Pixel),
                    new SolidBrush(Color.Blue), imageWidth + leftPadding * 3, imageHeight / 3 + upperPadding);
            }
            else
            {// center medical place name
                graphics.DrawString(medicalPlaceName, new Font("Helvetica", 40, FontStyle.Bold, GraphicsUnit.Pixel),
                    new SolidBrush(Color.Blue), pageWidth / 2, imageHeight / 3 + upperPadding, alignmentCenter);
            }
        }

        private void DrawPrescriptionBody(string patientName, string doctorName, DateTime date, 
                            string[] medicines, List<List<string>> medicinesAlternatives)
        {
            graphics.DrawString(newLine + new String('_', pageWidth / 17), bold, new SolidBrush(Color.Red),
                pageWidth / 2, 0, alignmentCenter);
            newLine += "\n";

            graphics.DrawString(newLine + "Name", bold, new SolidBrush(Color.Black),
                MeasureLeftPadding("Patient Name", patientName), 0, alignmentCenter);
            graphics.DrawString(newLine + "Date", bold, new SolidBrush(Color.Black),
                MeasureRightPadding("Date", date.ToString()), 0, alignmentRight);
            newLine += "\n\n";

            graphics.DrawString(newLine + patientName, regular, new SolidBrush(Color.Black), padding, 0);
            graphics.DrawString(newLine + date.ToString(), regular, new SolidBrush(Color.Black),
                                                                pageWidth - padding, 0, alignmentRight);
            newLine += "\n";

            if(medicines[0] != "")
            {
                if(medicinesAlternatives.Count == 0)
                {
                    graphics.DrawString(newLine + "Medicines", bold,
                                    new SolidBrush(Color.Black), padding, 0);
                }
                else
                {
                    graphics.DrawString(newLine + "Medicines\t\t            Alternatives", bold,
                                    new SolidBrush(Color.Black), padding, 0);
                }
                
                newLine += "\n\n";
            }

            for (int medicineIndex = 0; medicineIndex < medicines.Length && 
                medicines[medicineIndex] != ""; medicineIndex++)
            {
                newLine += "\n";
                graphics.DrawString(newLine + "- " + medicines[medicineIndex], regular,
                    new SolidBrush(Color.Black), padding * 5, 0);

                // write alternatives for each medicine
                if(medicinesAlternatives.Count != 0)
                {
                    for (int alternativeIndex = 0; alternativeIndex < medicinesAlternatives[medicineIndex].Count; alternativeIndex++)
                    {
                        graphics.DrawString(newLine + "- "+ medicinesAlternatives[medicineIndex][alternativeIndex]
                            , regular, new SolidBrush(Color.Black), pageWidth - padding * 11, 0, alignmentRight);
                        newLine += "\n";
                    }
                }
            }

            graphics.DrawString(newLine + "Doctor Name", bold, new SolidBrush(Color.Black),
                MeasureLeftPadding("Doctor Name", doctorName), 0, alignmentCenter);
            graphics.DrawString(newLine + "Signature", bold, new SolidBrush(Color.Black),
                MeasureRightPadding("Signature", date.ToString()), 0, alignmentRight);
            newLine += "\n";

            graphics.DrawString("\n\n\n" + newLine + doctorName, regular, new SolidBrush(Color.Black), padding * 2, 0);
            graphics.DrawString("\n"+newLine + new String('_', pageWidth/17), bold, new SolidBrush(Color.Red),
                pageWidth / 2, 0, alignmentCenter);
            newLine += "\n\n\n\n\n\n\n\n";
        }

        private void DrawPrescriptionFooter(string address, string phone)
        {
            regular = new Font("Helvetica", 17, FontStyle.Regular, GraphicsUnit.Pixel);
            bold = new Font("Helvetica", 21, FontStyle.Bold, GraphicsUnit.Pixel);

            graphics.DrawString(newLine + "Address", bold, new SolidBrush(Color.Black),
                MeasureLeftPadding("Address", address), 0, alignmentCenter);
            graphics.DrawString(newLine + "Phone", bold, new SolidBrush(Color.Black),
                MeasureRightPadding("Phone", phone), 0, alignmentRight);
            newLine += "\n\n\n\n\n\n\n";

            graphics.DrawString(newLine + address, regular, new SolidBrush(Color.Black), padding * 2, 0);
            graphics.DrawString(newLine + phone, regular, new SolidBrush(Color.Black),
                            pageWidth - padding, 0, alignmentRight);

        }

        private int MeasureRightPadding(string headWord, string dataword)
        {
            if (dataword == null)
                return pageWidth - padding * 10;

            return pageWidth - (int)graphics.MeasureString(dataword.ToString(), regular).Width / 2 +
                    (int)graphics.MeasureString(headWord, bold).Width / 2 - padding;
        }

        private int MeasureLeftPadding(string headWord, string dataWord)
        {
            int headWordWidth = (int)graphics.MeasureString(headWord, bold).Width;
            int dataWordWidth = (int)graphics.MeasureString(dataWord, regular).Width;
            if (dataWordWidth > headWordWidth)
                return dataWordWidth / 2 + padding;

            return headWordWidth / 2 + padding;
        }

    }
}