using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Xml.Linq;
using RmsAuto.Common.Properties;

namespace RmsAuto.Common.Data
{

    /// <summary>
    /// Класс для формирования Excel ввиде xml файла
    /// </summary>
    public class XMLExcel
    {
        XDocument _data;
        XElement _rootEX;
        XElement _workTable;
        XNamespace _o = "urn:schemas-microsoft-com:office:office";
        XNamespace _x = "urn:schemas-microsoft-com:office:excel";
        XNamespace _ss = "urn:schemas-microsoft-com:office:spreadsheet";
        XNamespace _html = "http://www.w3.org/TR/REC-html40";

        /// <summary>
        /// Создание объекта формирования xml файла
        /// </summary>
        /// <param name="workSheetName">Имя листа</param>
        public XMLExcel( string workSheetName )
        {
            _data = new XDocument( new XDeclaration( "1.0", "", "" ) );
            _data.AddAnnotation( "mso-application progid=\"Excel.Sheet\"" );
            _rootEX = new XElement( _ss + "Workbook",
                new XAttribute( "xmlns", _ss.NamespaceName ),
                new XAttribute( XNamespace.Xmlns + "o", _o.NamespaceName ),
                new XAttribute( XNamespace.Xmlns + "x", _x.NamespaceName ),
                new XAttribute( XNamespace.Xmlns + "ss", _ss.NamespaceName ),
                new XAttribute( XNamespace.Xmlns + "html", _html.NamespaceName ),
                new XElement( _o + "DocumentProperties",
                    new XAttribute( "xmlns", _o.NamespaceName ),
                    new XElement( _o + "Author", " " ),
                    new XElement( _o + "Version", "12.00" ) ),
                new XElement( _x + "ExcelWorkbook",
                    new XAttribute( "xmlns", _x.NamespaceName ),
                    new XElement( _x + "ProtectStructure", "False" ),
                    new XElement( _x + "ProtectWindows", "False" ) ),
                XElement.Parse( Resources.XMLExcelStyle ).FirstNode,
                new XElement( _ss + "Worksheet", new XAttribute( _ss + "Name", workSheetName ),
                _workTable = new XElement( _ss + "Table",
                    new XAttribute( _x + "FullColumns", "1" ),
                    new XAttribute( _x + "FullRows", "1" ),
                    new XAttribute( _ss + "DefaultRowHeight", "15" ) ) ) );
            _data.AddFirst( _rootEX );
        }

        /// <summary>
        /// Инициализации шапки и колонок файла с успешно загруженными строками Excel
        /// </summary>
        /// <param name="numberOrder">номер заказа</param>
        /// <param name="clientName">имя клиента</param>
        public void InitOKImportMess( string numberOrder, string clientName, string currency )
        {
            _workTable.Add( new XElement( _ss + "Column",
                   new XAttribute( _ss + "AutoFitWidth", "0" ),
                   new XAttribute( _ss + "Width", "33.75" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "AutoFitWidth", "0" ),
                new XAttribute( _ss + "Width", "87" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "AutoFitWidth", "0" ),
                new XAttribute( _ss + "Width", "95.25" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "AutoFitWidth", "0" ),
                new XAttribute( _ss + "Width", "111.75" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "AutoFitWidth", "0" ),
                new XAttribute( _ss + "Width", "51.75" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "Width", "135.75" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "AutoFitWidth", "0" ),
                new XAttribute( _ss + "Width", "90" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "AutoFitWidth", "0" ),
                new XAttribute( _ss + "Width", "71.25" ) ) );
            AddEmptyRow();
            _workTable.Add( new XElement( _ss + "Row",
                new XAttribute( _ss + "AutoFitHeight", "0" ),
                AddCell(),
                AddCell( "s71", "String", "Order No" ),
                AddCell( "s72", "String", numberOrder ) ) );
            _workTable.Add( new XElement( _ss + "Row",
                new XAttribute( _ss + "AutoFitHeight", "0" ),
                AddCell(),
                AddCell( "s73", "String", "Customer name" ),
                AddCell( "s74", "String", clientName ) ) );
            AddEmptyRow();
            // 81 - крайне левый 82 - средний 83 - крайне правый
            _workTable.Add( new XElement( _ss + "Row",
                new XAttribute( _ss + "AutoFitHeight", "0" ),
                AddCell( "s81", "String", "No" ),
                AddCell( "s82", "String", "Supplier ID" ),
                AddCell( "s82", "String", "Brand" ),
                AddCell( "s82", "String", "Part No" ),
                AddCell( "s82", "String", "Qnt" ),
                AddCell( "s82", "String", "Supersession is not applicable" ),
                AddCell( "s82", "String", "Reference ID" ),
                AddCell("s83", "String", "Price" + (!String.IsNullOrEmpty(currency) ? ", " + currency : ""))));
        }

        /// <summary>
        /// Инициализации шапки и колонок файла с не загруженными строками Excel
        /// </summary>
        /// <param name="numberOrder">номер заказа</param>
        /// <param name="clientName">имя клиента</param>
        public void InitErrorImportMess( string numberOrder, string clientName )
        {
            _workTable.Add( new XElement( _ss + "Column",
                   new XAttribute( _ss + "AutoFitWidth", "0" ),
                   new XAttribute( _ss + "Width", "33.75" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "AutoFitWidth", "0" ),
                new XAttribute( _ss + "Width", "87" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "AutoFitWidth", "0" ),
                new XAttribute( _ss + "Width", "95.25" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "AutoFitWidth", "0" ),
                new XAttribute( _ss + "Width", "111.75" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "AutoFitWidth", "0" ),
                new XAttribute( _ss + "Width", "51.75" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "Width", "135.75" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "AutoFitWidth", "0" ),
                new XAttribute( _ss + "Width", "90" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "AutoFitWidth", "0" ),
                new XAttribute( _ss + "Width", "71.25" ) ) );
            _workTable.Add( new XElement( _ss + "Column",
                new XAttribute( _ss + "AutoFitWidth", "0" ),
                new XAttribute( _ss + "Width", "296.25" ) ) );
            AddEmptyRow();
            _workTable.Add( new XElement( _ss + "Row",
                new XAttribute( _ss + "AutoFitHeight", "0" ),
                AddCell(),
                AddCell( "s71", "String", "Order No" ),
                AddCell( "s72", "String", numberOrder ) ) );
            _workTable.Add( new XElement( _ss + "Row",
                new XAttribute( _ss + "AutoFitHeight", "0" ),
                AddCell(),
                AddCell( "s73", "String", "Customer name" ),
                AddCell( "s74", "String", clientName ) ) );
            AddEmptyRow();
            // 81 - крайне левый 82 - средний 83 - крайне правый
            _workTable.Add( new XElement( _ss + "Row",
                new XAttribute( _ss + "AutoFitHeight", "0" ),
                AddCell( "s81", "String", "No" ),
                AddCell( "s82", "String", "Supplier ID" ),
                AddCell( "s82", "String", "Brand" ),
                AddCell( "s82", "String", "Part No" ),
                AddCell( "s82", "String", "Qnt" ),
                AddCell( "s82", "String", "Supersession is not applicable" ),
                AddCell( "s82", "String", "Reference ID" ),
                AddCell( "s82", "String", "Price" ),
                AddCell( "s83", "String", "Loading error" ) ) );
        }

        /// <summary>
        /// Добавление пустой строки в xml
        /// </summary>
        public void AddEmptyRow()
        {
            _workTable.Add( new XElement( _ss + "Row",
                new XAttribute( _ss + "AutoFitHeight", "0" ) ) );
        }

        /// <summary>
        /// Добавление строки в таблицу
        /// </summary>
        /// <param name="isTop">Это первая строка</param>
        /// <param name="isBotton">Это последняя строка</param>
        /// <param name="rowData">Данные строки</param>
        public void AddImportCartRow( bool isTop, bool isBotton, XMLExcelCell[] rowData )
        {
            XElement row = new XElement( _ss + "Row",
               new XAttribute( _ss + "AutoFitHeight", "0" ) );
            for ( int i = 0; i < rowData.Count(); i++ )
            {
                row.Add( AddCell( rowData[i], isTop, isBotton, i == 0, i == rowData.Count() - 1 ) );
            }
            _workTable.Add( row );
        }

        /// <summary>
        /// Создание ячейки
        /// </summary>
        /// <param name="Style">Имя стиля</param>
        /// <param name="valueType">Имя типа</param>
        /// <param name="valueData">Данные ячейки</param>
        /// <returns>Ячейка</returns>
        private XElement AddCell( string Style, string valueType, object valueData )
        {
            return new XElement( _ss + "Cell",
                    new XAttribute( _ss + "StyleID", Style ),
                    new XElement( _ss + "Data", new XAttribute( _ss + "Type", valueType ), valueData.ToString() ) );
        }

        /// <summary>
        /// Создание ячейки
        /// </summary>
        /// <param name="excelCell">Данные ячейки</param>
        /// <param name="isTop">Это первая строка</param>
        /// <param name="isBotton">Это последняя строка</param>
        /// <param name="isLeft">Это первая ячейка в строке</param>
        /// <param name="isRight">Это последняя ячейка в строке</param>
        /// <returns>Созданная ячейка</returns>
        private XElement AddCell( XMLExcelCell excelCell, bool isTop, bool isBotton, bool isLeft, bool isRight )
        {
            return new XElement( _ss + "Cell",
                    new XAttribute( _ss + "StyleID", GetStyleID( excelCell, isTop, isBotton, isLeft, isRight ) ),
                    new XElement( _ss + "Data", new XAttribute( _ss + "Type", ConvertType( excelCell.CellType ) ), excelCell.CellValue ) );
        }
        
        /// <summary>
        /// Создание пустой ячейки
        /// </summary>
        /// <returns>Созданная ячейка</returns>
        private XElement AddCell( )
        {
            return new XElement( _ss + "Cell" );
        }

        /// <summary>
        /// Преобразование созданного файла в массив байтов
        /// </summary>
        /// <returns>Массив байтов</returns>
        public byte[] ToByteArray()
        {
            string header = @"<?xml version=""1.0""?>
<?mso-application progid=""Excel.Sheet""?>
";
            Encoding enc = Encoding.GetEncoding( "UTF-8" );
            return enc.GetBytes( header + _data.ToString() );
        }

        /// <summary>
        /// Получения кода стиля по данным ячейки
        /// </summary>
        /// <param name="excelCell">Данные ячейки</param>
        /// <param name="isTop">Это первая строка</param>
        /// <param name="isBotton">Это последняя строка</param>
        /// <param name="isLeft">Это первая ячейка в строке</param>
        /// <param name="isRight">Это последняя ячейка в строке</param>
        /// <returns>Код ячейки универсальной разметки</returns>
        private string GetStyleID( XMLExcelCell excelCell, bool isTop, bool isBotton, bool isLeft, bool isRight )
        {
            int styleID = 0;
            switch ( excelCell.CellType )
            {
                case XMLExcelCellType.String: styleID = 105;
                    break;
                case XMLExcelCellType.Number: styleID = 115;
                    break;
                case XMLExcelCellType.Money: styleID = 125;
                    break;
                default: styleID = 105;
                    break;
            }
            if ( isTop ) styleID -= 3;
            if ( isBotton ) styleID += 3;
            if ( isLeft ) styleID -= 1;
            if ( isRight ) styleID += 1;
            styleID += (int)excelCell.cellAlignment;
            return "s" + styleID.ToString();
        }

        /// <summary>
        /// Конвертирование типа ячейки в строковое обозначение
        /// </summary>
        /// <param name="cellType">Тип ячейки</param>
        /// <returns>Наименование типа ячейки</returns>
        private string ConvertType( XMLExcelCellType cellType )
        {
            switch ( cellType )
            {
                case XMLExcelCellType.String: return "String";
                case XMLExcelCellType.Number: return "Number";
                case XMLExcelCellType.Money: return "Number";
                default: return "String";
            }
        }

    }

    /// <summary>
    /// Ячейка с данными
    /// </summary>
    public class XMLExcelCell
    {
        /// <summary>
        /// Тип ячейки
        /// </summary>
        public XMLExcelCellType CellType { get; set; }
        /// <summary>
        /// Выравнивание ячейки
        /// </summary>
        public XMLExcelAlignment cellAlignment { get; set; }
        /// <summary>
        /// Данные ячейки
        /// </summary>
        public string CellValue { get; set; }
    }

    /// <summary>
    /// Типы ячеек
    /// </summary>
    public enum XMLExcelCellType
    {
        String = 0,
        Number = 1,
        Money = 2
    }

    /// <summary>
    /// Типы выравниваний
    /// </summary>
    public enum XMLExcelAlignment
    {
        Left = 0,
        Center = 100,
        Right = 200
    }
}
