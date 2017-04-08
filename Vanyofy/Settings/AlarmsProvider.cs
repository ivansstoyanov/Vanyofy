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

        public Alarm Add(Alarm newAlarm)
        {
            if (newAlarm.Id == null)
            {
                newAlarm.Id = Guid.NewGuid();
                var json = new JavaScriptSerializer().Serialize(newAlarm);

                Properties.Settings.Default.Alarms.Add(json);
                Properties.Settings.Default.Save();
            }
            else
            {
                Edit(newAlarm);
            }

            return newAlarm;
        }

        public void Edit(Alarm newAlarm)
        {
            this.Delete(newAlarm.Id.Value);
            newAlarm.Id = null;
            newAlarm.Active = false;
            newAlarm.NotActive = true;

            this.Add(newAlarm);
        }

        public void SetActive(Alarm newAlarm)
        {
            var defaultSettings = Properties.Settings.Default.Alarms;
            StringCollection tmpSettings = new StringCollection();

            foreach (var setting in defaultSettings)
            {
                var currentAlarm = new JavaScriptSerializer().Deserialize<Alarm>(setting);
                if (currentAlarm.Id != newAlarm.Id.Value)
                {
                    tmpSettings.Add(setting);
                }
                else
                {
                    var json = new JavaScriptSerializer().Serialize(newAlarm);

                    tmpSettings.Add(json);
                }
            }

            this.ResetAll();

            Properties.Settings.Default.Alarms = tmpSettings;
            Properties.Settings.Default.Save();
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

        public void UpdateOrder(List<Guid> newOrder)
        {
            var defaultSettings = Properties.Settings.Default.Alarms;
            StringCollection newOrderSettings = new StringCollection();

            foreach (var itemId in newOrder)
            {
                foreach (var setting in defaultSettings)
                {
                    var currentAlarm = new JavaScriptSerializer().Deserialize<Alarm>(setting);
                    if (currentAlarm.Id == itemId)
                    {
                        newOrderSettings.Add(setting);
                        break;
                    }
                }
            }

            this.ResetAll();

            Properties.Settings.Default.Alarms = newOrderSettings;
            Properties.Settings.Default.Save();
        }
    }
}
