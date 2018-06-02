using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PatientCardCanvas
    {
        private Bitmap bitmap;
        private Graphics graphics;
        private int pageWidth, pageHeight;
        private SolidBrush WhiteBruch, BlueBruch;

        public PatientCardCanvas()
        {
            // page size
            pageHeight = 244;
            pageWidth = 375;

            // construct empty card 
            bitmap = new Bitmap(pageWidth, pageHeight);
            graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White); // white background

            // Drawing in high quality
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            
            // initialize bruches colors
            WhiteBruch = new SolidBrush(Color.White);
            BlueBruch = new SolidBrush(ColorTranslator.FromHtml("#0D47A1"));
        }

        public Bitmap Draw(Citizen citizen, Bitmap QRCode, Bitmap logo, Bitmap photo)
        {
            int headerHeight = 46, footerHeight = 42;
            int logoHeight = 34, logoWidth = 34;
            int imageSectionHeight = 120, patientPhotoWidth = 120, QRCodeWidth = 139;
            StringFormat alignmentCenter = new StringFormat();
            alignmentCenter.Alignment = StringAlignment.Center;

            // Header
            graphics.FillRectangle(BlueBruch, 0, 0, pageWidth, headerHeight);// background color
            graphics.DrawImage(logo, 16, 6, logoWidth, logoHeight);
            graphics.DrawString("Carepoint", new Font("Artifika", 14, FontStyle.Bold, GraphicsUnit.Pixel), WhiteBruch, 58, 13);
            graphics.DrawString("Patient card", new Font("Allerta", 20, FontStyle.Bold, GraphicsUnit.Pixel), WhiteBruch, 215, 10);

            // Body
            graphics.DrawImage(photo, 28, 68, patientPhotoWidth, imageSectionHeight);
            graphics.DrawLine(new Pen(BlueBruch, 3), 178, 72, 178, 184);
            graphics.DrawImage(QRCode,209,68,QRCodeWidth,imageSectionHeight);

            // Footer
            graphics.FillRectangle(BlueBruch, 0, 202, pageWidth, footerHeight);// background color
            graphics.DrawString(citizen.Name, new Font("Allerta", 17, FontStyle.Bold, 
                GraphicsUnit.Pixel), WhiteBruch, pageWidth/2, 210, alignmentCenter);

            graphics.Flush();
            graphics.Dispose();

            return bitmap;
        }
    }
}
