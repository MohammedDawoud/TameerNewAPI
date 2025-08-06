using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace TaamerProject.API.Helper
{
    public class RoundRectangle : IPdfPCellEvent
    {
        public void CellLayout(PdfPCell cell, System.Drawing.Rectangle rect, PdfContentByte[] canvas)
        {

            PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
            cb.SetRGBColorStroke(255, 0, 0);
            cb.RoundRectangle(
             rect.Left + 1.5f,
             rect.Bottom + 1.5f,
             rect.Width - 3,
             rect.Height - 3, 4
            );
            cb.Stroke();
        }

        public void CellLayout(PdfPCell cell, iTextSharp.text.Rectangle position, PdfContentByte[] canvases)
        {
        }
    }
}