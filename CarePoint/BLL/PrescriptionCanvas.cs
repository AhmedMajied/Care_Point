using System;
using System.Collections.Generic;
using System.Drawing;
using DAL;

namespace BLL
{
    public class PrescriptionCanvas
    {
        private string newLine, extraNewLine;
        private int pageWidth, pageHeight, padding;
        private int logoWidth, logoHeight;
        private Font regular, bold;
        private SolidBrush BlackBruch, BlueBrush, GreenBrush;
        private Bitmap bitmap;
        private Graphics graphics;
        private StringFormat alignmentCenter, alignmentRight;

        public PrescriptionCanvas()
        {
            newLine = extraNewLine = "";
            padding = 10;
            logoWidth = 50;
            logoHeight = 40;

            // initialize fonts
            regular = new Font("Helvetica", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            bold = new Font("Helvetica", 14, FontStyle.Bold, GraphicsUnit.Pixel);

            // initialize Bruches Colors
            BlackBruch = new SolidBrush(Color.Black);
            BlueBrush = new SolidBrush(ColorTranslator.FromHtml("#0D47A1"));
            GreenBrush = new SolidBrush(ColorTranslator.FromHtml("#006C31"));

            bitmap = new Bitmap(1, 1);
            graphics = Graphics.FromImage(bitmap);

            // alignment positions (center,right)
            alignmentCenter = new StringFormat();
            alignmentCenter.Alignment = StringAlignment.Center;
            alignmentRight = new StringFormat();
            alignmentRight.Alignment = StringAlignment.Far;
        }

        /// <summary>
        /// draw prescription that includes medicines were written to patient
        /// with these doses and these alternatives
        /// </summary>
        /// <returns>prescription image</returns>
        public Bitmap Draw(HistoryRecord historyRecord, string[] medicines,
            List<List<string>> medicinesAlternatives,string [] doses)
        {
            // decide page structure to know the sutable legth of it 
            string pageStructure = "\n\n\n\nName \n Patient name \n\n Medicines " + "\n\n"
                    + " Doctor \n Doctor name \n\n\n Address \n medicalPlaceAddress\n\n";
            for (int i = 0; i < medicinesAlternatives.Count; i++)
            {
                for(int c=0;c < medicinesAlternatives[i].Count;c++)
                    pageStructure += "\n";
                pageStructure += "\n";
            }

            pageWidth = 500;
            pageHeight = (int)graphics.MeasureString(pageStructure, bold).Height + logoHeight;

            bitmap = new Bitmap(bitmap, new Size(pageWidth, pageHeight));
            graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White); // white background

            // Drawing
            DrawPrescriptionHeader(historyRecord.MedicalPlace.Name, historyRecord.MedicalPlace.Photo);
            DrawPrescriptionBody(historyRecord.Citizen.Name, historyRecord.Specialist.Name,
                historyRecord.Date, medicines,medicinesAlternatives,doses);
            DrawPrescriptionFooter(historyRecord.MedicalPlace.Address, historyRecord.MedicalPlace.Phone);
            
            graphics.Flush();
            graphics.Dispose();

            return bitmap;
        }
        
        /// <summary>
        /// draw prescription header that includes medical place name and its logo
        /// </summary>
        private void DrawPrescriptionHeader(string medicalPlaceName, byte[] medicalPlaceImg)
        {
            int upperPadding = 10;
            Font largeFont = new Font("Helvetica", 16, FontStyle.Bold, GraphicsUnit.Pixel);

            if (medicalPlaceImg != null)
            {
                int widthOfHeader = (int)graphics.MeasureString(medicalPlaceName,largeFont).Width +
                        padding*2+logoWidth ;
                int leftPadding = (pageWidth - widthOfHeader) / 2;
                Image image = (Bitmap)((new ImageConverter()).ConvertFrom(medicalPlaceImg));
                
                graphics.DrawImage(image, new Rectangle(leftPadding, upperPadding,logoWidth, logoHeight),0,0,image.Width,image.Height,GraphicsUnit.Pixel);
                graphics.DrawString(medicalPlaceName, largeFont,
                    BlueBrush, leftPadding+logoWidth+padding * 2, logoHeight / 3 + upperPadding);
            }
            else
            {// center medical place name
                graphics.DrawString(medicalPlaceName, new Font("Helvetica", 16, FontStyle.Bold, GraphicsUnit.Pixel),
                    BlueBrush, pageWidth / 2, logoHeight / 3 + upperPadding, alignmentCenter);
            }

            newLine += "\n\n\n";
            graphics.DrawString(newLine + new String('_', 60),
                bold, BlueBrush, pageWidth / 2, 0, alignmentCenter);
            newLine += "\n\n";
        }

        /// <summary>
        /// draw prescription body that includes patient name, doctor name, 
        /// medicines, their alternatives and their doses
        /// </summary>
        private void DrawPrescriptionBody(string patientName, string doctorName, DateTime date, 
                            string[] medicines, List<List<string>> medicinesAlternatives, string[] doses)
        {
            string combinedAlternatives = "", alternativesNewLines = "", currentAlternativesLine;
            int wordWidthInPixels;
                
            graphics.DrawString(newLine + "Name" , bold, BlackBruch,
                MeasureLeftPadding("Patient Name", patientName), 0, alignmentCenter);
            graphics.DrawString(newLine + "Date", bold, BlackBruch,
                MeasureRightPadding("Date", date.ToString()), 0, alignmentRight);
            newLine += "\n";
            extraNewLine += "\n";

            graphics.DrawString(newLine+extraNewLine + patientName,
                                        regular, BlackBruch, padding, 0);
            graphics.DrawString(newLine+extraNewLine + date.ToString(), regular,
                                BlackBruch,pageWidth - padding, 0, alignmentRight);
            newLine += "\n\n";
            
            if(medicines[0] != "")
            {
                graphics.DrawString(newLine + "Medicines", bold,BlueBrush, padding, 0);
                newLine += "\n";
                extraNewLine += "\n";
            }
            
            // write medicines with their alternatives and their doses
            for (int medicineIndex = 0; medicineIndex < medicines.Length && 
                medicines[medicineIndex] != ""; medicineIndex++)
            {
                graphics.DrawString(newLine+extraNewLine + "- " + medicines[medicineIndex], regular,
                    BlackBruch, padding * 3, 0);
                newLine += "\n";

                // write alternatives for each medicine
                if(medicinesAlternatives[medicineIndex].Count != 0)
                {
                    combinedAlternatives = "Alternatives : ( " + medicinesAlternatives[medicineIndex][0];
                    currentAlternativesLine = combinedAlternatives;

                    for (int alternativeIndex = 1; alternativeIndex < medicinesAlternatives[medicineIndex].Count; alternativeIndex++)
                    {
                        wordWidthInPixels = (int)graphics.MeasureString(currentAlternativesLine += 
                            medicinesAlternatives[medicineIndex][alternativeIndex],regular).Width;

                        if (wordWidthInPixels >= pageWidth-padding*5)
                        {
                            combinedAlternatives += "\n\t";
                            alternativesNewLines += "\n";
                            currentAlternativesLine = "\t";
                        }
                        combinedAlternatives += ", "+medicinesAlternatives[medicineIndex][alternativeIndex];
                    }

                    combinedAlternatives += " )";
                    graphics.DrawString(newLine+extraNewLine + combinedAlternatives, regular, BlueBrush, padding*5, 0);
                    newLine += "\n"+alternativesNewLines;
                    alternativesNewLines = "";
                }
                // write doses
                graphics.DrawString(newLine + extraNewLine +"Dose : "+doses[medicineIndex], regular,
                    GreenBrush, padding * 5, 0);
                newLine += "\n";
            }

            newLine += "\n";
            graphics.DrawString(newLine + "Doctor Name", bold, BlackBruch,
                MeasureLeftPadding("Doctor Name", doctorName), 0, alignmentCenter);
            graphics.DrawString(newLine + "Signature", bold, BlackBruch,
                MeasureRightPadding("Signature", date.ToString()), 0, alignmentRight);
            newLine += "\n";
            extraNewLine += "\n";

            graphics.DrawString(newLine+extraNewLine + doctorName, regular, BlackBruch, padding, 0);
            newLine += "\n";
        }

        /// <summary>
        /// draw prescription footer that includes medical place address and its phone
        /// </summary>
        private void DrawPrescriptionFooter(string address, string phone)
        {
            Font smallBold = new Font("Helvetica", 12, FontStyle.Bold, GraphicsUnit.Pixel);

            graphics.DrawString(newLine + new String('_', 60), bold, BlueBrush,
                pageWidth / 2, 0, alignmentCenter);
            newLine += "\n\n";

            graphics.DrawString(newLine+extraNewLine + "Address", smallBold, BlackBruch,
                MeasureLeftPadding("Address", address), 0, alignmentCenter);
            graphics.DrawString(newLine+extraNewLine + "Phone", smallBold, BlackBruch,
                MeasureRightPadding("Phone", phone)-6, 0, alignmentRight);
            newLine += "\n\n";

            graphics.DrawString(newLine+extraNewLine + address, regular, BlackBruch, padding, 0);
            graphics.DrawString(newLine+extraNewLine + phone, regular, BlackBruch,
                            pageWidth - padding, 0, alignmentRight);
        }

        /// <summary>
        /// measure right padding according to words length 
        /// to make data word centered under head word
        /// </summary>
        private int MeasureRightPadding(string headWord, string dataword)
        {
            if (dataword == null)
                return pageWidth - padding * 5;

            return pageWidth - (int)graphics.MeasureString(dataword.ToString(), regular).Width / 2
                    +(int)graphics.MeasureString(headWord, bold).Width / 2 + 1- padding;
        }

        /// <summary>
        /// measure left padding according to words length 
        /// to make data word centered under head word
        /// </summary>
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