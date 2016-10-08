using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BassinoBase.Controllers
{
    class EventsReport : PdfPageEventHelper
    {
        public string Title { get; set; }

        //public override void OnEndPage(PdfWriter writer, Document document)
        //{
        //    PdfPTable table = new PdfPTable(1);
        //    //table.WidthPercentage = 100; //PdfPTable.writeselectedrows below didn't like this
        //    table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin; //this centers [table]
        //    PdfPTable table2 = new PdfPTable(2);
        //    string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Images/logobassino.png");
        //    //logo
        //    var imagen = Image.GetInstance(path);
        //    imagen.ScaleAbsolute(200,100);

        //    PdfPCell cell2 = new PdfPCell(imagen);
        //    cell2.Colspan = 1;

        //    cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    table2.AddCell(cell2);

        //    //title
        //    cell2 = new PdfPCell(new Phrase("\nTITLE", new Font(Font.NORMAL, 16, Font.BOLD | Font.UNDERLINE)));
        //    cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //    cell2.Colspan = 1;
        //    table2.AddCell(cell2);

        //    PdfPCell cell = new PdfPCell(table2);
        //    table.AddCell(cell);

        //    table.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 36, writer.DirectContent);
        //}
    }
    public class ITextEvents1 : PdfPageEventHelper
    {

        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;


        #region Fields
        private string _header;
        #endregion

        #region Properties

        public string Titulo { get; set; }
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion

        public override void OnStartPage(PdfWriter writer, Document document)
        {

        }
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {

            }
            catch (System.IO.IOException ioe)
            {

            }
        }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {

            PrintTime = DateTime.Now;
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb = writer.DirectContent;
            headerTemplate = cb.CreateTemplate(100, 100);
            footerTemplate = cb.CreateTemplate(50, 50);
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Images/logobassino.png");
            //logo
            var imagen = Image.GetInstance(path);
            imagen.ScaleAbsolute(100, 50);
            base.OnEndPage(writer, document);

            iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font baseFontSecond = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            Phrase p1Header = new Phrase("CARLOS BASSINO", baseFontNormal);
            Phrase p2Header = new Phrase("Servicios de Logística y Distribución", baseFontSecond);

            //Create PdfTable object
            PdfPTable pdfTab = new PdfPTable(3);

            //We will have to create separate cells to include image logo and 2 separate strings
            //Row 1
            PdfPCell pdfCell1 = new PdfPCell(p1Header);
            PdfPCell pdfCell2 = new PdfPCell();
            PdfPCell pdfCell8 = new PdfPCell(p2Header);
            PdfPCell pdfCell9 = new PdfPCell();
            PdfPCell pdfCell3 = new PdfPCell(imagen);
            String text = "Pagina " + writer.PageNumber ;


            //Add paging to header
            //{
            //    cb.BeginText();
            //    cb.SetFontAndSize(bf, 12);
            //    cb.SetTextMatrix(document.PageSize.GetRight(200), document.PageSize.GetTop(45));
            //    cb.ShowText(text);
            //    cb.EndText();
            //    float len = bf.GetWidthPoint(text, 12);
            //    //Adds "12" in Page 1 of 12
            //    cb.AddTemplate(headerTemplate, document.PageSize.GetRight(200) + len, document.PageSize.GetTop(45));
            //}
            //Add paging to footer
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetTextMatrix(document.PageSize.GetRight(100), document.PageSize.GetBottom(30));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 12);
                cb.AddTemplate(footerTemplate, document.PageSize.GetRight(180) + (len), document.PageSize.GetBottom(30));
            }
            //Row 2
            PdfPCell pdfCell4 = new PdfPCell(new Phrase(Titulo, baseFontNormal));
            //Row 3


            PdfPCell pdfCell5 = new PdfPCell(new Phrase("Fecha: " + PrintTime.ToShortDateString(), baseFontBig));
            PdfPCell pdfCell6 = new PdfPCell();
            PdfPCell pdfCell7 = new PdfPCell(new Phrase("Hora: " + string.Format("{0:t}", DateTime.Now), baseFontBig));


            //set the alignment of all three cells and set border to 0
            pdfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell4.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell5.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell7.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell7.HorizontalAlignment = Element.ALIGN_RIGHT;


            pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfCell3.VerticalAlignment = Element.ALIGN_TOP;
            pdfCell4.VerticalAlignment = Element.ALIGN_TOP;
            pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell8.VerticalAlignment = Element.ALIGN_MIDDLE;


            pdfCell4.Colspan = 3;
            pdfCell8.Colspan = 3;
            pdfCell3.Rowspan = 2;



            pdfCell1.Border = 0;
            pdfCell2.Border = 0;
            pdfCell3.Border = 0;
            pdfCell4.Border = 0;
            pdfCell5.Border = 0;
            pdfCell6.Border = 0;
            pdfCell7.Border = 0;
            pdfCell8.Border = 0;
            pdfCell9.Border = 0;


            //add all three cells into PdfTable
            pdfTab.AddCell(pdfCell1);
            pdfTab.AddCell(pdfCell2);
            pdfTab.AddCell(pdfCell3);
            pdfTab.AddCell(pdfCell8);
           pdfTab.AddCell(pdfCell4);
           
            pdfTab.AddCell(pdfCell5);
            pdfTab.AddCell(pdfCell6);
            pdfTab.AddCell(pdfCell7);
            //pdfTab.AddCell(pdfCell8);
            //pdfTab.AddCell(pdfCell9);

            pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.WidthPercentage = 70;
            //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;


            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            //set pdfContent value

            //Move the pointer and draw line to separate header section from rest of page
            cb.MoveTo(40, document.PageSize.Height - 130);
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 130);
            cb.Stroke();

            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(40, document.PageSize.GetBottom(50));
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            cb.Stroke();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 12);
            headerTemplate.SetTextMatrix(0, 0);
            headerTemplate.ShowText((writer.PageNumber - 1).ToString());
            headerTemplate.EndText();

            //footerTemplate.BeginText();
            //footerTemplate.SetFontAndSize(bf, 12);
            //footerTemplate.SetTextMatrix(0, 0);
            //footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            //footerTemplate.EndText();


        }
    }
    public class itextEvents : PdfPageEventHelper
    {

        //Create object of PdfContentByte
        PdfContentByte pdfContent;

        public void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            //We are going to add two strings in header. Create separate Phrase object with font setting and string to be included
            Phrase p1Header = new Phrase("BlueLemonCode generated page", FontFactory.GetFont("verdana", 8));
            Phrase p2Header = new Phrase("confidential", FontFactory.GetFont("verdana", 8));
            //create iTextSharp.text Image object using local image path
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Images/logobassino.png");
            iTextSharp.text.Image imgPDF = iTextSharp.text.Image.GetInstance(path);

            //Create PdfTable object
            PdfPTable pdfTab = new PdfPTable(3);
            //We will have to create separate cells to include image logo and 2 separate strings
            PdfPCell pdfCell1 = new PdfPCell(imgPDF);
            PdfPCell pdfCell2 = new PdfPCell(p1Header);
            PdfPCell pdfCell3 = new PdfPCell(p2Header);
            //set the alignment of all three cells and set border to 0
            pdfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell1.Border = 0;
            pdfCell2.Border = 0;
            pdfCell3.Border = 0;
            //add all three cells into PdfTable
            pdfTab.AddCell(pdfCell1);
            pdfTab.AddCell(pdfCell2);
            pdfTab.AddCell(pdfCell3);
            pdfTab.TotalWidth = document.PageSize.Width - 20;
            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 10, document.PageSize.Height - 15, writer.DirectContent);
            //set pdfContent value
            pdfContent = writer.DirectContent;
            //Move the pointer and draw line to separate header section from rest of page
            pdfContent.MoveTo(30, document.PageSize.Height - 35);
            pdfContent.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 35);
            pdfContent.Stroke();
        }
    }
    public class PDFFooter : PdfPageEventHelper
    {

        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            tabFot.SpacingAfter = 10F;
            PdfPCell cell;
            tabFot.TotalWidth = 300F;
            cell = new PdfPCell(new Phrase("Header"));
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, 150, document.Top, writer.DirectContent);
        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            PdfPCell cell;
            tabFot.TotalWidth = 300F;
            cell = new PdfPCell(new Phrase("Footer"));
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, 150, document.Bottom, writer.DirectContent);
        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }
    }

}