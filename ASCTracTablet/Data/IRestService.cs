using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ASCTracTablet.Data
{
    public interface IRestService
    {
        Task<ASCTracFunctionStruct.SignonType> Signon(ASCTracFunctionStruct.inputType aInputType);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> Signoff(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);


        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPOStatusSummary(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPOStatusByPO(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetConfirmReceiptInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doConfirmReceiptInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> PrintPOReport(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPOInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPOList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);

        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetMaintInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> SetMaintInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> ImageCapture(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);

        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetDockSchd(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doDockSchd(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doNewDockSchd(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);

        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOStatusSummary(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOStatusByCO(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);

        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);

        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> PrintOrderContainer(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> CalcPCE(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> UpdateOrdrDet(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetInvAvail(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);

        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetScheduleInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doScheduleInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetContainerList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);

        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOReportInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doCOReport(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);


        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetUsersCOEntryInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOEntryInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> CompleteCOEntry(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> COEntrySaveHeader(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> COEntrySaveDetail(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);




        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetBOMAvailList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetInvList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetSkidInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetSkidHistory(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> UpdateSkid(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);

        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCounts(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPhysLocitems(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPhysLocs(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> RecountPhys(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> PhysCount(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);



        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOStatusSummary(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOStatusByWO(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);


        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOHdrInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOHdrList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> ScheduleWO(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetProdSkidList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> ProdNewSkid(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);

        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOComponents(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOComponentLicenses(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> WOIssueComponent(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);



        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetQCTests(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetQCTestInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doQCTest(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetSkidQCInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);

        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> ToggleQCSkid(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);


        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetReplenInfoForZone(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);
        Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetReplenSummary(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg);

    }
}

