using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASCTracTablet.Data
{
	public class RestManager
	{
		IRestService restService;

		public RestManager(IRestService service)
		{
			restService = service;
		}

		public Task<ASCTracFunctionStruct.SignonType> Signon(ASCTracFunctionStruct.inputType aInputType)
		{
			return restService.Signon(aInputType);
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> Signoff(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.Signoff(aInboundMsg));
		}


		// Receiving
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPOStatusSummary(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetPOStatusSummary(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPOStatusByPO(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetPOStatusByPO(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetConfirmReceiptInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetConfirmReceiptInfo(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doConfirmReceiptInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.doConfirmReceiptInfo(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> PrintPOReport(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.PrintPOReport(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPOInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetPOInfo(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPOList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetPOList(aInboundMsg));
		}



		//Misc Functions
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetMaintInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetMaintInfo(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> SetMaintInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.SetMaintInfo(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> ImageCapture(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.ImageCapture(aInboundMsg));
		}


		// Dock Schedule
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetDockSchd(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetDockSchd(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doDockSchd(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.doDockSchd(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doNewDockSchd(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.doNewDockSchd(aInboundMsg));
		}

		// CO Lookup

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOStatusSummary(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetCOStatusSummary(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOStatusByCO(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetCOStatusByCO(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetCOInfo(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetCOList(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> PrintOrderContainer(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.PrintOrderContainer(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> CalcPCE(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.CalcPCE(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> UpdateOrdrDet(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.UpdateOrdrDet(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetInvAvail(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetInvAvail(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetScheduleInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.UpdateOrdrDet(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doScheduleInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.UpdateOrdrDet(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetContainerList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetContainerList(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOReportInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetCOReportInfo(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doCOReport(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.doCOReport(aInboundMsg));
		}


		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetUsersCOEntryInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetUsersCOEntryInfo(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOEntryInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetCOEntryInfo(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> COEntrySaveHeader(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.COEntrySaveHeader(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> COEntrySaveDetail(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.COEntrySaveDetail(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> CompleteCOEntry(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.CompleteCOEntry(aInboundMsg));
		}

		// Inventory Lookup functions
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetBOMAvailList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
			return restService.GetBOMAvailList(aInboundMsg);
        }
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetInvList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetInvList(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetSkidInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetSkidInfo(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetSkidHistory(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetSkidHistory(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> UpdateSkid(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.UpdateSkid(aInboundMsg));
		}


		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCounts(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetCounts(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPhysLocitems(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetPhysLocitems(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPhysLocs(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetPhysLocs(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> RecountPhys(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.RecountPhys(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> PhysCount(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.PhysCount(aInboundMsg));
		}


		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOStatusByWO(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
			return restService.GetWOStatusByWO(aInboundMsg);
        }
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOStatusSummary(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
			return restService.GetWOStatusSummary(aInboundMsg);

		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOHdrInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetWOHdrInfo(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOHdrList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetWOHdrList(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> ScheduleWO(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.ScheduleWO(aInboundMsg));
		}

		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetProdSkidList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetProdSkidList(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> ProdNewSkid(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.ProdNewSkid(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOComponents(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetWOComponents(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOComponentLicenses(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetWOComponentLicenses(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> WOIssueComponent(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
			return restService.WOIssueComponent(aInboundMsg);
		}


		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetQCTests(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetQCTests(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetQCTestInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetQCTestInfo(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doQCTest(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.doQCTest(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> ToggleQCSkid(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.ToggleQCSkid(aInboundMsg));
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetSkidQCInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return (restService.GetSkidQCInfo(aInboundMsg));
		}



		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetReplenInfoForZone(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return restService.GetReplenInfoForZone(aInboundMsg);
		}
		public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetReplenSummary(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
		{
			return restService.GetReplenSummary(aInboundMsg);
		}
	}
}
