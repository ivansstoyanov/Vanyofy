using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Vanyofy.Models;

namespace Vanyofy.Settings
{
    public class AlarmsProvider
    {
        public AlarmsProvider()
        {

        }

        public List<Alarm> GetAll()
        {
            var defaultSettings = Properties.Settings.Default.Alarms;
            List<Alarm> alarms = new List<Alarm>();

            foreach (var setting in defaultSettings)
            {
                var currentAlarm = new JavaScriptSerializer().Deserialize<Alarm>(setting);
                alarms.Add(currentAlarm);
            }

            return alarms;
        }

        public void Add(Alarm newAlarm)
        {
            var json = new JavaScriptSerializer().Serialize(newAlarm);

            Properties.Settings.Default.Alarms.Add(json);
            Properties.Settings.Default.Save();
        }

        public void Edit(Alarm newAlarm)
        {
            this.Delete(newAlarm.Id);

            this.Add(newAlarm);
        }

        public void Delete(Guid id)
        {
            var defaultSettings = Properties.Settings.Default.Alarms;
            StringCollection tmpSettings = new StringCollection();

            foreach (var setting in defaultSettings)
            {
                var currentAlarm = new JavaScriptSerializer().Deserialize<Alarm>(setting);
                if(currentAlarm.Id != id)
                {
                    tmpSettings.Add(setting);
                }
            }

            this.ResetAll();

            Properties.Settings.Default.Alarms = tmpSettings;
            Properties.Settings.Default.Save();
        }

        public void ResetAll()
        {
            Properties.Settings.Default.Reset();
        }
    }
}
