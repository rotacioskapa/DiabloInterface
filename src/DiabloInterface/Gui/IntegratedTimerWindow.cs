using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiabloInterface.Gui
{
    public partial class IntegratedTimerWindow : Form
    {
        public const string TimeFormat = "hh\\:mm\\:ss\\.fff";
        TimeSpan? time 
        {
            get
            {
                return (DateTime.Now - startTime);
            }
        }

        DateTime? startTime;

        MainWindow _main;
        List<TimedItem> items = new List<TimedItem>();
        public IntegratedTimerWindow(MainWindow main)
        {
            InitializeComponent();
            _main = main;

            Init();
        }

        public void Init()
        {
            startTime = null;

            items.Clear();
            foreach (var item in _main.settings.autosplits)
            {
                items.Add(new TimedItem
                {
                    Id = item.GetHashCode(),
                    Name = item.name,
                    reached = item.reached,
                    Time = null
                });
            }

            listBox1.Items.Clear();
            foreach (var item in items)
            {
                listBox1.Items.Add(item);
            }
        }

        public void UpdateItems()
        {
            listBox1.Items.Clear();
            foreach (var item in items)
            {
                listBox1.Items.Add(item);
            }
        }

        private void updateTicker_Tick(object sender, EventArgs e)
        {
            if (startTime == null)
            {
                if (_main.settings.autosplits.Count(x => x.reached == true) == 1)
                {
                    startTime = DateTime.Now;
                }
            }
            else
            {
                if (_main.settings.autosplits.All(x => x.reached == true))
                {
                    updateTicker.Enabled = false;
                }
            }

            var current = _main.settings.autosplits.FirstOrDefault(x => x.reached == false);
            if (current != null)
            {
                var currentTimed =  items.FirstOrDefault(x => x.Id == current.GetHashCode());
                if (currentTimed != null)
                    currentTimed.Time = time.HasValue ? time.Value.ToString(TimeFormat) : "";
            }

            label1.Text = time.HasValue ? time.Value.ToString(TimeFormat) : "-";
        }
    }

    public class TimedItem
    {
        public long? Id { get; set; }
        public string Time { get; set; }
        public bool reached { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return string.Format("{0} \t {1}", Name, Time);
        }
    }
}
