using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Web;

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
            regular = new Font("Times New Roman", 26, FontStyle.Regular, GraphicsUnit.Pixel);
            bold = new Font("Times New Roman", 30, FontStyle.Bold, GraphicsUnit.Pixel);

            bitmap = new Bitmap(1, 1);
            graphics = Graphics.FromImage(bitmap);

            // alignment positions
            alignmentCenter = new StringFormat();
            alignmentCenter.Alignment = StringAlignment.Center;

            alignmentRight = new StringFormat();
            alignmentRight.Alignment = StringAlignment.Far;
        }

        public Bitmap convertTextToImage()
        {
            // Dummy data
            string medicine1 = "Medicine 1";
            string medicine2 = "Medicine 2";
            string patientName = "Ahmed Mohamed Ahmed";
            string doctorName = "Emad Nabil";
            string address = "Shubra street Cairo Egypt";
            string phone = "";
            string medicalPlaceName = "Medical Place Name";
            DateTime date = new DateTime();

            string pageStructure = "\n\n\nName \n Patient name \n Medicines " + "\n"
                    + medicine1 + "\n" + medicine2 +
                    "\n Doctor \n Doctor name \n\n Address \n medicalPlaceAddress\n";

            pageWidth = 900;
            pageHeight = (int)graphics.MeasureString(pageStructure, bold).Height + imageHeight;

            bitmap = new Bitmap(bitmap, new Size(pageWidth, pageHeight));
            graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.White); // white background
            // Drawing in high quality
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.InterpolationMode = InterpolationMode.High;

            // Drawing
            DrawPrescriptionHeader(medicalPlaceName);
            DrawPrescriptionBody(patientName, doctorName, date, medicine1, medicine2);
            DrawPrescriptionFooter(address, phone);

            graphics.Flush();
            graphics.Dispose();

            return bitmap;
        }

        private void DrawPrescriptionHeader(string medicalPlaceName)
        {
            int leftPadding = 50;
            int upperPadding = 10;

            // Dummy
            Bitmap image1 = (Bitmap)Image.FromFile(@"F:\pic.png", true);
            ImageConverter converter = new ImageConverter();
            byte[] bytes = (byte[])converter.ConvertTo(image1, typeof(byte[]));

            Image image = (Bitmap)((new ImageConverter()).ConvertFrom(bytes));
            graphics.DrawImage(image, leftPadding, upperPadding, leftPadding + imageWidth, upperPadding + imageHeight);

            graphics.DrawString(medicalPlaceName, new Font("Times New Roman", 50, FontStyle.Bold, GraphicsUnit.Pixel),
                                new SolidBrush(Color.Blue), imageWidth + leftPadding * 3, imageHeight / 3 + upperPadding);

        }

        private void DrawPrescriptionBody(string patientName, string doctorName, DateTime date, string medicine1, string medicine2)
        {
            graphics.DrawString(newLine + new String('_', pageWidth / 22), bold, new SolidBrush(Color.Red),
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

            graphics.DrawString(newLine + "Medicines", bold, new SolidBrush(Color.Black), padding, 0);
            newLine += "\n\n\n";

            graphics.DrawString(newLine + "-" + medicine1, regular, new SolidBrush(Color.Black), padding * 10, 0);
            newLine += "\n";
            graphics.DrawString(newLine + "-" + medicine2, regular, new SolidBrush(Color.Black), padding * 10, 0);

            graphics.DrawString(newLine + "Doctor Name", bold, new SolidBrush(Color.Black),
                MeasureLeftPadding("Doctor Name", doctorName), 0, alignmentCenter);
            graphics.DrawString(newLine + "Signature", bold, new SolidBrush(Color.Black),
                MeasureRightPadding("Signature", date.ToString()), 0, alignmentRight);
            newLine += "\n\n";

            graphics.DrawString("\n" + newLine + doctorName, regular, new SolidBrush(Color.Black), padding * 2, 0);
            graphics.DrawString(newLine + new String('_', pageWidth / 22), bold, new SolidBrush(Color.Red),
                pageWidth / 2, 0, alignmentCenter);
            newLine += "\n\n\n\n";
        }

        private void DrawPrescriptionFooter(string address, string phone)
        {
            regular = new Font("Times New Roman", 20, FontStyle.Regular, GraphicsUnit.Pixel);
            bold = new Font("Times New Roman", 25, FontStyle.Bold, GraphicsUnit.Pixel);

            graphics.DrawString(newLine + "Address", bold, new SolidBrush(Color.Black),
                MeasureLeftPadding("Address", address), 0, alignmentCenter);
            graphics.DrawString(newLine + "Phone", bold, new SolidBrush(Color.Black),
                MeasureRightPadding("Phone", phone), 0, alignmentRight);
            newLine += "\n\n\n\n\n";

            graphics.DrawString(newLine + address, regular, new SolidBrush(Color.Black), padding * 2, 0);
            graphics.DrawString(newLine + phone, regular, new SolidBrush(Color.Black),
                            pageWidth - padding, 0, alignmentRight);

        }

        private int MeasureRightPadding(string headWord, string dataword)
        {
            if (dataword.Length == 0)
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
