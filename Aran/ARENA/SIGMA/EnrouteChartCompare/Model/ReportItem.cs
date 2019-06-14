namespace EnrouteChartCompare.Model
{
    class ReportItem
    {
        public string Feature { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Description { get; set; }
    }
}
