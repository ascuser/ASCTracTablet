using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SQLite;
using Xamarin.Forms;

namespace ASCTracTablet.Data
{
    public class ASCTracDB
    {

        readonly SQLiteAsyncConnection _connection;
        private string dbPath;
        public ASCTracDB()
        {
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ASCTrac.db3");

            _connection = new SQLiteAsyncConnection(dbPath);

            _connection.CreateTableAsync<DataModel.dataConfig>().Wait();
            _connection.CreateTableAsync<DataModel.dataMenuSetup>().Wait();

            _connection.CreateTableAsync<DataModel.dataReasonCode>().Wait();
            _connection.CreateTableAsync<DataModel.dataQCReasonType>().Wait();
            _connection.CreateTableAsync<DataModel.dataQCTest>().Wait();

            _connection.CreateTableAsync<DataModel.dataLocItems>().Wait();

            _connection.CreateTableAsync<DataModel.dataOrdrDet>().Wait();
            _connection.CreateTableAsync<DataModel.data3PLRate>().Wait();

            _connection.CreateTableAsync<DataModel.dataWODet>().Wait();

        }

        /*
        private SQLiteConnection _connection;

        public ASCTracDB()
        {
            _connection = DependencyService.Get<ISQLite>().GetConnection();

            _connection.CreateTable<DataModel.dataConfig>();
            //_connection.CreateTable<dataPrinter>();
        }
        */
        public void InitTables()
        {

        }

        #region dataConfig
        public Task<DataModel.dataConfig> GetConfig(string aCfgField)
        {
            return (_connection.Table<DataModel.dataConfig>().FirstOrDefaultAsync(t => t.cfgname == aCfgField));
        }
        public string GetConfigValue(string aCfgField)
        {
            string retval = string.Empty;
            var recVal = GetConfig(aCfgField).Result;
            if (recVal != null)
                retval = recVal.cfgValue;
            return (retval);
        }
        public string GetConfigValue(string aCfgField, string aDefaultVal)
        {
            string retval = aDefaultVal;
            var recVal = GetConfig(aCfgField).Result;
            if (recVal != null)
                retval = recVal.cfgValue;
            return (retval);
        }


        public int GetConfigIntValue(string aCfgField)
        {
            int retval = 0;
            var recVal = GetConfig(aCfgField).Result;
            if ((recVal != null) && (!String.IsNullOrEmpty(recVal.cfgValue)))
                retval = Convert.ToInt32(recVal.cfgValue);
            return (retval);
        }

        public void SaveConfig(string aCfgField, string acfgValue)
        {
            if (GetConfig(aCfgField) != null)
                _connection.DeleteAsync<DataModel.dataConfig>(aCfgField);
            var newRec = new DataModel.dataConfig
            {
                cfgname = aCfgField,
                cfgValue = acfgValue
            };
            _connection.InsertAsync(newRec);
        }
        public void SaveBoolConfig(string aCfgField, bool acfgValue)
        {
            string cfgvalue = "F";
            if (acfgValue)
                cfgvalue = "T";
            SaveConfig(aCfgField, cfgvalue);
        }
        #endregion

        #region data_MenuSetup
        public void ClearMenuSetup()
        {
            _connection.DeleteAllAsync<DataModel.dataMenuSetup>();
        }

        public void AddMenu(string aID, string aDescription, string aParentID)
        {
            var newRec = new DataModel.dataMenuSetup
            {
                MenuID = aID,
                Description = aDescription,
                MenuParentID = aParentID
            };
            _connection.InsertAsync(newRec);
        }

        public IEnumerable<DataModel.dataMenuSetup> GetMenuList(string aParentID)
        {
            // return (from t in _connection.Table<dataMenuSetup>()
            //         select t).ToList();
            var result = _connection.Table<DataModel.dataMenuSetup>().Where(t => t.MenuParentID == aParentID).OrderBy(t => t.MenuID).ToListAsync();
            return result.Result;
        }

        public DataModel.dataMenuSetup GetMenuByID(string aID)
        {
            return (_connection.Table<DataModel.dataMenuSetup>().FirstOrDefaultAsync(t => t.MenuID == aID).Result);
        }
        public DataModel.dataMenuSetup GetMenuByDesc(string aDesc)
        {
            return (_connection.Table<DataModel.dataMenuSetup>().FirstOrDefaultAsync(t => t.Description == aDesc).Result);
        }

        public void fillMenuList(ASCTracFunctionStruct.SignonType aSignonType)
        {
            ClearMenuSetup();
            foreach (var rec in aSignonType.menuList)
            {
                string[] data = rec.Value.Split('|');
                AddMenu(rec.Key, data[0], data[1]);
            }
        }

        #endregion


        #region dataReasonCodes
        public IEnumerable<DataModel.dataReasonCode> GetReasonCodes()
        {
            return _connection.Table<DataModel.dataReasonCode>().Where(t => !String.IsNullOrEmpty(t.ReasonCode)).ToListAsync().Result;
            //return (from t in _connection.Table<DataModel.dataReasonCode>().Where ( t => !String.IsNullOrEmpty( t.ReasonCode ).ToListAsync();
        }
        public void ClearReasonCodes()
        {
            _connection.DeleteAllAsync<DataModel.dataReasonCode>();
        }

        public DataModel.dataReasonCode AddReasonCode(string aReasonCode, string aDescription, string aReasonType, string aMafFlag,
            string aDefaultCostCenter, string aAskCostCenter, string aAskRespSite, string aAskComments, string aAskProjectNumber)
        {
            var newRec = new DataModel.dataReasonCode
            {
                ReasonCode = aReasonCode,
                Description = aDescription,
                ReasonType = aReasonType,
                MAFFlag = aMafFlag,
                askCostCenter = aAskCostCenter,
                askRespSite = aAskRespSite,
                askComments = aAskComments,
                askProjectNumber = aAskProjectNumber,
                defaultCostCenter = aDefaultCostCenter
            };

            _connection.InsertAsync(newRec);
            return (newRec);
        }
        public DataModel.dataReasonCode GetReason(string aReasonCode)
        {
            return (_connection.Table<DataModel.dataReasonCode>().FirstOrDefaultAsync(t => t.ReasonCode == aReasonCode).Result);
        }

        public void fillReasonCodes(Dictionary<string, string> aList)
        {
            ClearReasonCodes();

            foreach (var rec in aList)
            {
                string[] data = rec.Value.Split('|');
                AddReasonCode(rec.Key, data[1], data[0], data[2], data[3], data[4], data[5], data[6], data[7]);
            }
        }

        //public dataQCReasonType GetQCReason(long aRefNum)
        //{
        //    return (_connection.Table<dataQCReasonType>().FirstOrDefault(t => t.RefNum == aRefNum));
        // }
        #endregion

        #region data_QCTests
        public void ClearQCTests()
        {
            _connection.DeleteAllAsync<DataModel.dataQCTest>();
        }

        public void AddQCTest(DataModel.dataQCTest aRec)
        {
            _connection.InsertAsync(aRec);
        }

        public IEnumerable<DataModel.dataQCTest> GetQCTests()
        {
            return (_connection.Table<DataModel.dataQCTest>().ToListAsync().Result);
        }

        public DataModel.dataQCTest GetQCTest(long aBatchNum)
        {
            return (_connection.Table<DataModel.dataQCTest>().Where(t => t.BatchNum == aBatchNum).FirstOrDefaultAsync().Result);
        }

        #endregion

        #region dataQCReasonType

        public List<DataModel.dataQCReasonType> GetQCReasons()
        {
            return (_connection.Table<DataModel.dataQCReasonType>().ToListAsync().Result);
        }

        public void ClearQCReason()
        {
            _connection.DeleteAllAsync<DataModel.dataQCReasonType>();
        }

        public DataModel.dataQCReasonType AddQCReason(DataModel.dataQCReasonType aRec)
        {
            _connection.InsertAsync(aRec);
            return (aRec);
        }

        //public void SaveQCReasons(System.Collections.ObjectModel.ObservableCollection<ascCOSM.ascTracWCFService.QCReasonType> hostQCReasonList)
        public void SaveQCReasons(List<ASCTracFunctionStruct.QC.QCReasonType> hostQCReasonList)
        {
            ClearQCReason();
            foreach (var rec in hostQCReasonList)
            {
                DataModel.dataQCReasonType newrec = new DataModel.dataQCReasonType();
                newrec.RefNum = rec.RefNum;
                newrec.Reason = rec.Reason;
                newrec.ReasonDescription = rec.ReasonDescription;
                newrec.OnHold = rec.OnHold;
                newrec.MafStatus = rec.MafStatus;
                newrec.MafNum = rec.MafNum;
                newrec.MafDescription = rec.MafDescription;
                newrec.MafCatID = rec.MafCatID;
                newrec.MafAction = rec.MafAction;
                newrec.HoldDatetime = rec.HoldDatetime;
                newrec.ExpectedReleaseDate = rec.ExpectedReleaseDate;
                newrec.fNewRec = false;
                AddQCReason(newrec);
            }
        }

        #endregion

        #region data3PLReasonCodes
        public void Clear3PLRates()
        {
            _connection.DeleteAllAsync<DataModel.data3PLRate>();
        }

        public void Add3PLRate(string aID, string aDescription, double afee)
        {
            var newRec = new DataModel.data3PLRate
            {
                ID = aID,
                Description = aDescription,
                fee = afee
            };
            _connection.InsertAsync(newRec);
            //return (newRec);
        }

        public IEnumerable<DataModel.data3PLRate> Get3PLRates()
        {
            return _connection.Table<DataModel.data3PLRate>().Where(t => !String.IsNullOrEmpty(t.ID)).ToListAsync().Result;
        }


        public DataModel.data3PLRate Get3PLRate(string aID)
        {
            return (_connection.Table<DataModel.data3PLRate>().FirstOrDefaultAsync(t => t.ID == aID).Result);
        }

        public void fill3PLRate(Dictionary<string, string> aList)
        {
            Clear3PLRates();

            foreach (var rec in aList)
            {
                string[] data = rec.Value.Split('|');
                double fee = 0;
                double.TryParse(data[1], out fee);
                Add3PLRate(rec.Key, data[0], fee);
            }
        }

        //public dataQCReasonType GetQCReason(long aRefNum)
        //{
        //    return (_connection.Table<dataQCReasonType>().FirstOrDefault(t => t.RefNum == aRefNum));
        // }
        #endregion

        #region dataOrdrDet
        public void ClearOrdrDet()
        {
            _connection.DeleteAllAsync<DataModel.dataOrdrDet>();
        }

        public void AddOrdrDet(DataModel.dataOrdrDet aRec)
        {
            _connection.InsertAsync(aRec);
        }

        public IEnumerable<DataModel.dataOrdrDet> GetOrdrDets()
        {
            return _connection.Table<DataModel.dataOrdrDet>().Where(t => !String.IsNullOrEmpty(t.ItemID)).ToListAsync().Result;
        }

        public DataModel.dataOrdrDet GetOrdrDet(long aLineNumber)
        {
            return (_connection.Table<DataModel.dataOrdrDet>().FirstOrDefaultAsync(t => t.LineNumber == aLineNumber).Result);
        }

        public void fillOrdrDet(string xmlStr)
        {
            var xdoc = System.Xml.Linq.XDocument.Parse(xmlStr).Element("NewDataSet"); //.Element( "Users");

            ClearOrdrDet();
            foreach (XElement xe in xdoc.Nodes())
            {
                //SEQ_NUM, D.COMP_ITEMID, I.DESCRIPTION, D.QTY, D.QTY_PICKED, D.QTY_USED
                long linenum = Convert.ToInt64(ascUtils.XGetString(xe.Element("LINENUMBER")));
                DataModel.dataOrdrDet rec = new DataModel.dataOrdrDet();
                rec.LineNumber = linenum;
                rec.ItemID = ascUtils.XGetString(xe.Element("ITEMID"));
                rec.Description = ascUtils.XGetString(xe.Element("DESCRIPTION"));
                rec.QtyOrdered = ascUtils.XGetDouble(xe.Element("QTYORDERED"));
                rec.QtyPicked = ascUtils.XGetDouble(xe.Element("QTYPICKED"));
                rec.QtyLoaded = ascUtils.XGetDouble(xe.Element("QTYLOADED"));
                rec.PickLocation = ascUtils.XGetString(xe.Element("PICK_LOCATION"));
                rec.OrderFilled = ascUtils.XGetString(xe.Element("ORDERFILLED"));
                rec.PCEType = ascUtils.XGetString(xe.Element("PCE_TYPE"));

                AddOrdrDet(rec);
            }
        }
        #endregion

        #region dataLocItems
        public void ClearLocItems()
        {
            _connection.DeleteAllAsync<DataModel.dataLocItems>();
        }

        public void AddLocItems(DataModel.dataLocItems aRec)
        {
            _connection.InsertAsync(aRec);
        }

        public IEnumerable<DataModel.dataLocItems> GetLocItems()
        {
            return _connection.Table<DataModel.dataLocItems>().Where(t => !String.IsNullOrEmpty(t.SkidID)).ToListAsync().Result;
        }

        public DataModel.dataLocItems GetSkid(string aSkidID)
        {
            return (_connection.Table<DataModel.dataLocItems>().FirstOrDefaultAsync(t => t.SkidID == aSkidID).Result);
        }
        public void UpdateSkid(string aSkidID, double aQty, string aLocation)
        {
            var rec = GetSkid(aSkidID);
            rec.QtyTotal = aQty;
            if (!String.IsNullOrEmpty(aLocation))
                rec.LocationID = aLocation;
            _connection.UpdateAsync(rec);
        }
        public void UpdateInv(string aItemId, string aLocationID, double aQty, string aNewLocation)
        {
            string updStr = "QtyTotal=" + aQty.ToString();
            if (!String.IsNullOrEmpty(aNewLocation))
                updStr += ", LocationID='" + aNewLocation + "'";
            _connection.ExecuteAsync("UPDATE dataLocItems SET " + updStr + " WHERE LocationID=? and ItemID=?", aLocationID, aItemId);
        }

        public void fillInventory(string xmlStr)
        {
            var xdoc = System.Xml.Linq.XDocument.Parse(xmlStr).Element("NewDataSet"); //.Element( "Users");

            ClearLocItems();
            foreach (XElement xe in xdoc.Nodes())
            {
                //SEQ_NUM, D.COMP_ITEMID, I.DESCRIPTION, D.QTY, D.QTY_PICKED, D.QTY_USED
                DataModel.dataLocItems rec = new DataModel.dataLocItems();
                rec.SkidID = ascUtils.XGetString(xe.Element("SKIDID"));
                rec.ItemID = ascUtils.XGetString(xe.Element("ITEMID"));
                rec.ItemDescription = ascUtils.XGetString(xe.Element("DESCRIPTION"));
                rec.QtyTotal = ascUtils.XGetDouble(xe.Element("QTYTOTAL"));
                double dTmp = ascUtils.XGetDouble(xe.Element("QTYONHOLD"));
                if (dTmp > 0)
                    rec.QAHold = "T";
                else
                    rec.QAHold = "F";
                rec.ReasonForHold = ascUtils.XGetString(xe.Element("REASONFORHOLD"));
                rec.LocationID = ascUtils.XGetString(xe.Element("LOCATIONID"));
                rec.LotID = ascUtils.XGetString(xe.Element("LOTID"));
                string sTmp = ascUtils.XGetString(xe.Element("EXPDATE"));
                if (!String.IsNullOrEmpty(sTmp))
                    rec.ExpDate = Convert.ToDateTime(sTmp);
                sTmp = ascUtils.XGetString(xe.Element("DATETIMEPROD"));
                if (!String.IsNullOrEmpty(sTmp))
                    rec.DateTimeProd = Convert.ToDateTime(sTmp);
                rec.Validated = (rec.QtyTotal > 0);

                rec.preallocOrderNum = ascUtils.XGetString(xe.Element("PREALLOC_ORDERNUMBER"));
                rec.preallocWorkorderID = ascUtils.XGetString(xe.Element("PREALLOC_WORKORDER_ID"));
                rec.promoCode = ascUtils.XGetString(xe.Element("PROMO_CODE"));
                rec.reblendFlag = ascUtils.XGetString(xe.Element("REBLEND_FLAG"));
                rec.loctype = ascUtils.XGetString(xe.Element("TYPE"));
                rec.locPickType = ascUtils.XGetString(xe.Element("PICK_ASSIGNMENT_FLAG"));
                rec.locPickableFlag = ascUtils.XGetString(xe.Element("PICKABLE_FLAG"));

                if (rec.SkidID.Equals("-"))
                    rec.SkidID = "-" + rec.LocationID;

                AddLocItems(rec);
            }

        }

        #endregion

        #region data_WODet
        public void ClearWODet()
        {
            _connection.DeleteAllAsync<DataModel.dataWODet>();
        }

        public void AddWODet(DataModel.dataWODet aRec)
        {
            _connection.InsertAsync(aRec);
        }

        public List<DataModel.dataWODet> GetWODets()
        {
            return (_connection.Table<DataModel.dataWODet>().ToListAsync().Result);
        }

        public DataModel.dataWODet GetWODet(long aSeqNum)
        {
            var result = _connection.Table<DataModel.dataWODet>().Where(t => t.SeqNum == aSeqNum).FirstOrDefaultAsync().Result;
            return (result);
        }

        #endregion



    }
}