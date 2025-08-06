
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Helpers;

namespace TaamerProject.API.pdfHandler
{
    public class reportHelper
    {
        public static byte[] PopulateChart(List<chartDataClass> chartData)
        {
            string themeChart = @"<Chart>
                      <ChartAreas>
                        <ChartArea Name=""Default"" _Template_=""All"">
                          <AxisY>
                            <LabelStyle Font=""Verdana, 12px"" />
                          </AxisY>
                          <AxisX LineColor=""64, 64, 64, 64"" Interval=""1"">
                            <LabelStyle Font=""Verdana, 12px"" />
                          </AxisX>
                        </ChartArea>
                      </ChartAreas>
                    </Chart>";
            Chart chart = new Chart(width: 1000, height: 300, theme: themeChart);
            chart.SetYAxis("الإنجاز", 0, 100);
            chart.AddTitle("نسبة إنجاز المشاريع");
            chart.AddSeries("Default", chartType: "Column", xValue: chartData, xField: "ProjectNo", yValues: chartData, yFields: "Percentage");
            chart.SetYAxis("المشروع", 0, 100);
            chart.SetYAxis("الإنجاز", 0, 100);
            return chart.GetBytes(format: "png");
        }
        public static byte[] PopulateChartForCost(List<chartDataClass> chartData)
        {
            string themeChart = @"<Chart>
                      <ChartAreas>
                        <ChartArea Name=""Default"" _Template_=""All"">
                          <AxisY>
                            <LabelStyle Font=""Verdana, 12px"" />
                          </AxisY>
                          <AxisX LineColor=""64, 64, 64, 64"" Interval=""1"">
                            <LabelStyle Font=""Verdana, 12px"" />
                          </AxisX>
                        </ChartArea>
                      </ChartAreas>
                    </Chart>";
            Chart chart = new Chart(width: 1000, height: 300, theme: themeChart);
            // chart.SetYAxis("التكلفة", 0, 100);
            chart.AddTitle("تكلفة المشاريع");
            chart.AddSeries("Default", chartType: "Column", xValue: chartData, xField: "ProjectNo", yValues: chartData, yFields: "Percentage");
            //chart.SetYAxis("الإنجاز", 0, 100);
            return chart.GetBytes(format: "png");
        }
    }

}
