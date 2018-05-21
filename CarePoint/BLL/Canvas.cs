﻿using System;
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
        private string newLine, extraNewLine;
        private int pageWidth, pageHeight, padding;
        private int logoWidth, logoHeight;
        private Font regular, bold;
        private SolidBrush BlackBruch, BlueBrush;
        private Bitmap bitmap;
        private Graphics graphics;
        private StringFormat alignmentCenter, alignmentRight;

        public Canvas()
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

            bitmap = new Bitmap(1, 1);
            graphics = Graphics.FromImage(bitmap);

            // alignment positions (center,right)
            alignmentCenter = new StringFormat();
            alignmentCenter.Alignment = StringAlignment.Center;
            alignmentRight = new StringFormat();
            alignmentRight.Alignment = StringAlignment.Far;
        }

        public Bitmap drawText(HistoryRecord historyRecord, string[] medicines,
            List<List<string>> medicinesAlternatives)
        {
            // decide page structure to know the sutable legth of it 
            string pageStructure = "\n\n\n\nName \n Patient name \n\n Medicines " + "\n\n"
                    + " Doctor \n Doctor name \n\n\n Address \n medicalPlaceAddress";

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

            // Drawing in high quality
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;

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

        private void DrawPrescriptionBody(string patientName, string doctorName, DateTime date, 
                            string[] medicines, List<List<string>> medicinesAlternatives)
        {
            string combinedAlternatives = "", alternativesNewLines = "", currentAlternativesLine = "";
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
                        wordWidthInPixels = (int)graphics.MeasureString(currentAlternativesLine + 
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

        private void DrawPrescriptionFooter(string address, string phone)
        {
            Font smallBold = new Font("Helvetica", 12, FontStyle.Bold, GraphicsUnit.Pixel);

            graphics.DrawString(newLine + new String('_', 60), bold, BlueBrush,
                pageWidth / 2, 0, alignmentCenter);
            newLine += "\n";

            graphics.DrawString(newLine+extraNewLine + "Address", smallBold, BlackBruch,
                MeasureLeftPadding("Address", address), 0, alignmentCenter);
            graphics.DrawString(newLine+extraNewLine + "Phone", smallBold, BlackBruch,
                MeasureRightPadding("Phone", phone)-6, 0, alignmentRight);
            newLine += "\n\n";

            graphics.DrawString(newLine+extraNewLine + address, regular, BlackBruch, padding, 0);
            graphics.DrawString(newLine+extraNewLine + phone, regular, BlackBruch,
                            pageWidth - padding, 0, alignmentRight);
        }

        private int MeasureRightPadding(string headWord, string dataword)
        {
            if (dataword == null)
                return pageWidth - padding * 5;

            return pageWidth - (int)graphics.MeasureString(dataword.ToString(), regular).Width / 2
                    +(int)graphics.MeasureString(headWord, bold).Width / 2 + 1- padding;
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