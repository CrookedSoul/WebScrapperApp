using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraperApp.Model.Chapter
{
    public class ParameterModel
    {
        public string ChapterUrl { get; set; }
        public double PageWidth { get; set; }
        public double PageHeight { get; set; }
        public double Density { get; set; }
        public double? FontSize { get; set; }
    }
}
