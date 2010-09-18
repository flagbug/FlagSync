using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlagSync2.Core;

namespace FlagSync2.View
{
    public partial  class JobConfigurationPanel : UserControl
    {
        private Job job;

        public Job Job
        {
            get
            {
                return this.job;
            }

            set
            {
                this.job = value;
                this.UpdateJobConfiguration();
            }
        }

        public event EventHandler JobNameChanged;
        protected virtual void OnJobNameChanged()
        {
            if(this.JobNameChanged != null)
            {
                this.JobNameChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public JobConfigurationPanel()
        {
            InitializeComponent();
        }

        protected virtual void UpdateJobConfiguration()
        {

        }
    }
}
