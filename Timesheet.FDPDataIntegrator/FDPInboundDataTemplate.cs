﻿namespace Timesheet.FDPDataIntegrator
{
    public enum IntegrationType
    {
        EMPLOYEE, PAYROLL
    }

    internal class FDPInboundDataTemplate
    {
        private const string ModificationDateParam = "@ModificationDate";
        private const string PageNumberParam = "@PageNumber";

        private static string _defaultInboudDataTemplate => $@"
            <FPDTS>
                <InboundData>
                    <Records>
                        <Record>
                            <ModifiedAfter>{ModificationDateParam}</ModifiedAfter>
                            <PageNumber>{PageNumberParam}</PageNumber>
                            <PageSize>15000</PageSize>
                        </Record>
                    </Records>
                </InboundData>
            </FPDTS>
        ";

        private static string _employeeTransfertName = "WilsonFire-Resource-InExport";
        private static string _payrollTransfertName = "WilsonFire-TimesheetDetails-InExport";

        internal static (string transfertName, string inboundData) GetIntegrationParams(IntegrationType type, int days)
        {
            var fromDate = DateTime.Now.AddDays(-days);
            //var fromDate = from ?? DateTime.Now.AddYears(-5);

            var inboundData = _defaultInboudDataTemplate.Replace(ModificationDateParam, fromDate.ToString("dd MMM yyy HH':'mm':'ss"));
            inboundData = inboundData.Replace(PageNumberParam, "1");

            var transfertName = type == IntegrationType.EMPLOYEE ? _employeeTransfertName : _payrollTransfertName;

            return (transfertName, inboundData);
        }
    }
}
