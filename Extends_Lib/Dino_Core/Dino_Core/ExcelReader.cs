using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dino_Core
{
    public static class ExcelReader
    {
        /// <summary>
        /// 读取EXCEL文件
        /// </summary>
        /// <param name="_filePath"></param>
        /// <returns></returns>
        public static DataSet ReadExcel(string _filePath)
        {
            try
            {
                FileStream stream = File.Open(_filePath, FileMode.Open, FileAccess.Read);

                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                DataSet result = excelReader.AsDataSet();

                return result;
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return null;
            
        }
    }
}
