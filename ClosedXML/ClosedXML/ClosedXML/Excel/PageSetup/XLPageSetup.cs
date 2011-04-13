﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClosedXML.Excel
{
    internal class XLPageSetup : IXLPageSetup
    {
        public XLPageSetup(IXLPageSetup defaultPageOptions, XLWorksheet worksheet)
        {
            
            if (defaultPageOptions != null)
            {
                this.PrintAreas = new XLPrintAreas(defaultPageOptions.PrintAreas as XLPrintAreas, worksheet);
                this.CenterHorizontally = defaultPageOptions.CenterHorizontally;
                this.CenterVertically = defaultPageOptions.CenterVertically;
                this.FirstPageNumber = defaultPageOptions.FirstPageNumber;
                this.HorizontalDpi = defaultPageOptions.HorizontalDpi;
                this.PageOrientation = defaultPageOptions.PageOrientation;
                this.VerticalDpi = defaultPageOptions.VerticalDpi;

                this.PaperSize = defaultPageOptions.PaperSize;
                this.pagesTall = defaultPageOptions.PagesTall;
                this.pagesWide = defaultPageOptions.PagesWide;
                this.scale = defaultPageOptions.Scale;

                if (defaultPageOptions.Margins != null)
                {
                    this.Margins = new XLMargins()
                            {
                                Top = defaultPageOptions.Margins.Top,
                                Bottom = defaultPageOptions.Margins.Bottom,
                                Left = defaultPageOptions.Margins.Left,
                                Right = defaultPageOptions.Margins.Right,
                                Header = defaultPageOptions.Margins.Header,
                                Footer = defaultPageOptions.Margins.Footer
                            };
                }
                this.AlignHFWithMargins = defaultPageOptions.AlignHFWithMargins;
                this.ScaleHFWithDocument = defaultPageOptions.ScaleHFWithDocument;
                this.ShowGridlines = defaultPageOptions.ShowGridlines;
                this.ShowRowAndColumnHeadings = defaultPageOptions.ShowRowAndColumnHeadings;
                this.BlackAndWhite = defaultPageOptions.BlackAndWhite;
                this.DraftQuality = defaultPageOptions.DraftQuality;
                this.PageOrder = defaultPageOptions.PageOrder;

                this.ColumnBreaks = defaultPageOptions.ColumnBreaks.ToList();
                this.RowBreaks = defaultPageOptions.RowBreaks.ToList();
                Header = new XLHeaderFooter(defaultPageOptions.Header as XLHeaderFooter);
                Footer = new XLHeaderFooter(defaultPageOptions.Footer as XLHeaderFooter);
                this.PrintErrorValue = defaultPageOptions.PrintErrorValue;
            }
            else
            {
                this.PrintAreas = new XLPrintAreas(worksheet);
                Header = new XLHeaderFooter();
                Footer = new XLHeaderFooter();
                this.ColumnBreaks = new List<Int32>();
                this.RowBreaks = new List<Int32>();
            }
        }
        public IXLPrintAreas PrintAreas { get; private set; }


        public Int32 FirstRowToRepeatAtTop { get; private set; }
        public Int32 LastRowToRepeatAtTop { get; private set; }
        public void SetRowsToRepeatAtTop(String range)
        {
            var arrRange = range.Replace("$", "").Split(':');
            SetRowsToRepeatAtTop(Int32.Parse(arrRange[0]), Int32.Parse(arrRange[1]));
        }
        public void SetRowsToRepeatAtTop(Int32 firstRowToRepeatAtTop, Int32 lastRowToRepeatAtTop)
        {
            if (firstRowToRepeatAtTop <= 0) throw new ArgumentOutOfRangeException("The first row has to be greater than zero.");
            if (firstRowToRepeatAtTop > lastRowToRepeatAtTop) throw new ArgumentOutOfRangeException("The first row has to be less than the second row.");

            FirstRowToRepeatAtTop = firstRowToRepeatAtTop;
            LastRowToRepeatAtTop = lastRowToRepeatAtTop;
        }
        public Int32 FirstColumnToRepeatAtLeft { get; private set; }
        public Int32 LastColumnToRepeatAtLeft { get; private set; }
        public void SetColumnsToRepeatAtLeft(String range)
        {
            var arrRange = range.Replace("$", "").Split(':');
            Int32 iTest;
            if (Int32.TryParse(arrRange[0], out iTest))
                SetColumnsToRepeatAtLeft(Int32.Parse(arrRange[0]), Int32.Parse(arrRange[1]));
            else
                SetColumnsToRepeatAtLeft(arrRange[0], arrRange[1]);
        }
        public void SetColumnsToRepeatAtLeft(String firstColumnToRepeatAtLeft, String lastColumnToRepeatAtLeft)
        {
            SetColumnsToRepeatAtLeft(XLAddress.GetColumnNumberFromLetter(firstColumnToRepeatAtLeft), XLAddress.GetColumnNumberFromLetter(lastColumnToRepeatAtLeft));
        }
        public void SetColumnsToRepeatAtLeft(Int32 firstColumnToRepeatAtLeft, Int32 lastColumnToRepeatAtLeft)
        {
            if (firstColumnToRepeatAtLeft <= 0) throw new ArgumentOutOfRangeException("The first column has to be greater than zero.");
            if (firstColumnToRepeatAtLeft > lastColumnToRepeatAtLeft) throw new ArgumentOutOfRangeException("The first column has to be less than the second column.");

            FirstColumnToRepeatAtLeft = firstColumnToRepeatAtLeft;
            LastColumnToRepeatAtLeft = lastColumnToRepeatAtLeft;
        }

        public XLPageOrientation PageOrientation { get; set; }
        public XLPaperSize PaperSize { get; set; }
        public UInt32 HorizontalDpi { get; set; }
        public UInt32 VerticalDpi { get; set; }
        public Int32 FirstPageNumber { get; set; }
        public Boolean CenterHorizontally { get; set; }
        public Boolean CenterVertically { get; set; }
        public XLPrintErrorValues PrintErrorValue { get; set; }
        public IXLMargins Margins { get; set; }

        private Int32 pagesWide;
        public Int32 PagesWide 
        {
            get
            {
                return pagesWide;
            }
            set
            {
                pagesWide = value;
                if (pagesWide >0)
                    scale = 0;
            }
        }
        
        private Int32 pagesTall;
        public Int32 PagesTall
        {
            get
            {
                return pagesTall;
            }
            set
            {
                pagesTall = value;
                if (pagesTall >0)
                    scale = 0;
            }
        }

        private Int32 scale;
        public Int32 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                if (scale > 0)
                {
                    pagesTall = 0;
                    pagesWide = 0;
                }
            }
        }

        public void AdjustTo(Int32 pctOfNormalSize)
        {
            Scale = pctOfNormalSize;
            pagesWide = 0;
            pagesTall = 0;
        }
        public void FitToPages(Int32 pagesWide, Int32 pagesTall)
        {
            this.pagesWide = pagesWide;
            this.pagesTall = pagesTall;
            scale = 0;
        }


        public IXLHeaderFooter Header { get; private set; }
        public IXLHeaderFooter Footer { get; private set; }

        public Boolean ScaleHFWithDocument { get; set; }
        public Boolean AlignHFWithMargins { get; set; }

        public Boolean ShowGridlines { get; set; }
        public Boolean ShowRowAndColumnHeadings { get; set; }
        public Boolean BlackAndWhite { get; set; }
        public Boolean DraftQuality { get; set; }

        public XLPageOrderValues PageOrder { get; set; }
        public XLShowCommentsValues ShowComments { get; set; }

        public List<Int32> RowBreaks { get; private set; }
        public List<Int32> ColumnBreaks { get; private set; }
        public void AddHorizontalPageBreak(Int32 row)
        {
            if (!RowBreaks.Contains(row))
                RowBreaks.Add(row);
        }
        public void AddVerticalPageBreak(Int32 column)
        {
            if (!ColumnBreaks.Contains(column))
                ColumnBreaks.Add(column);
        }

        //public void SetPageBreak(IXLRange range, XLPageBreakLocations breakLocation)
        //{
        //    switch (breakLocation)
        //    {
        //        case XLPageBreakLocations.AboveRange: RowBreaks.Add(range.Internals.Worksheet.Row(range.RowNumber)); break;
        //        case XLPageBreakLocations.BelowRange: RowBreaks.Add(range.Internals.Worksheet.Row(range.RowCount())); break;
        //        case XLPageBreakLocations.LeftOfRange: ColumnBreaks.Add(range.Internals.Worksheet.Column(range.ColumnNumber)); break;
        //        case XLPageBreakLocations.RightOfRange: ColumnBreaks.Add(range.Internals.Worksheet.Column(range.ColumnCount())); break;
        //        default: throw new NotImplementedException();
        //    }
        //}
    }
}