﻿namespace Timesheet.FDPDataIntegrator
{
    internal class FDPSettings
    {
        public string FDP_Username { get; private set; }
        public string FDP_Password { get; private set; }
        public string FDP_Url { get; private set; }
        public string FDP_Domain { get; private set; }

        public static FDPSettings CreateFromConfigurationList(IEnumerable<(string Name, string Value)> configurations)
        {
            var fdpSettings = new FDPSettings();
            foreach (var setting in configurations)
            {
                var name = setting.Name;
                var value = setting.Value;

                if (nameof(FDP_Username).ToUpper() == name.ToUpper())
                {
                    fdpSettings.FDP_Username = value;
                }

                if (nameof(FDP_Password).ToUpper() == name.ToUpper())
                {
                    fdpSettings.FDP_Password = value;
                }

                if (nameof(FDP_Url).ToUpper() == name.ToUpper())
                {
                    fdpSettings.FDP_Url = value;
                }

                if (nameof(FDP_Domain).ToUpper() == name.ToUpper())
                {
                    fdpSettings.FDP_Domain = value;
                }
            }

            if (string.IsNullOrEmpty(fdpSettings.FDP_Username)
                || string.IsNullOrEmpty(fdpSettings.FDP_Password))
            {
                throw new Exception("FDP Configurations are not available");
            }

            return fdpSettings;
        }
    }
}
