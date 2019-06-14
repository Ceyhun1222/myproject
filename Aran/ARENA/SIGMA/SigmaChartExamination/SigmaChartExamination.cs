using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARENA;
using PDM;
using SigmaChartExamination;
using System.Windows.Forms;


namespace SigmaChart
{
    public class SigmaChartExamination
    {
        public int SigmaChartTypes;
        public DateTime _time;
        public string _projectName;
        public Dictionary<string, List<PDMObject>> ExaminationData;
        //List<PDMObject>

        public List<ExaminationMessage> ExaminationResults { get; set; }

        public SigmaChartExamination()
        {
        }

        public virtual void ChartExamination()
        {
        }

        public virtual void ShowExamResults()
        {

            if (this.ExaminationResults != null && this.ExaminationResults.Count > 0)
            {
                ExaminationresultsForm frm = new ExaminationresultsForm();
                {
                    frm.listBox1.Items.Clear();



                    var queryObjectsGroup = from ExamGroup in this.ExaminationResults group ExamGroup by new { Ftp = ExamGroup.PdmFeatureType, Fid = ExamGroup.PdmFeatureId, Flb = ExamGroup.PdmFeatureLabel } into GroupOfObjects orderby GroupOfObjects.Key.Ftp select GroupOfObjects;


                    foreach (var Grp in queryObjectsGroup)
                    {
                        frm.listBox1.Items.Add(Grp.Key.Ftp);
                        frm.listBox1.Items.Add(Grp.Key.Flb + " (" + Grp.Key.Fid + ")");
                        foreach (var _mes in Grp)
                        {
                            foreach (var item in _mes.ExaminationsDetails)
                            {
                                frm.listBox1.Items.Add((char)9 + item);
                            }
                        }
                    }

                   
                }

                frm.ShowDialog();
            }

        }

    }

    //public class ExaminationSection
    //{
    //    public string PdmFeatureType { get; set; }
    //    public List<ExaminationMessage> Section { get; set; }

    //    public ExaminationSection()
    //    {
    //    }
    //}

    public class ExaminationMessage
    {
        public string PdmFeatureType { get; set; }
        public string PdmFeatureId { get; set; }
        public string PdmFeatureLabel { get; set; }
        //public PDMObject pdmObj { get; set; }
        public List<string> ExaminationsDetails { get; set; }

        public ExaminationMessage()
        {
        }

    }

}
