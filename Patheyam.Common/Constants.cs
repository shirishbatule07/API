
namespace Patheyam.Common
{
    public static class StoredProcedureConstants
    {
        public static string GetCities = "spCommon_GetCities";
        public static string GetStates = "spCommon_GetStates";
        public static string GetCountries = "spCommon_GetCountries";
        public static string GetTitles = "spCommon_GetTitles";
        public static string GetLanguages = "spCommon_GetLanguages";
        public static string GetCurrencies = "spCommon_GetCurrencies";
        public static string GetTimeZones = "spCommon_GetTimeZones";
        public static string UpsertTimeZone = "spCommon_UpsertTimeZones";
        public static string DeleteTimeZone = "spCommon_DeleteTimeZone";
        public static string GetLocationByCityId = "spCommon_GetLocationByCityId";
        public static string DeleteTimeZonesByIds = "spCommon_DeleteTimeZonesByIds";

        public static string GetCurrencyById = "spCommon_GetCurrencyById";
        public static string UpsertCurrency = "spCommon_UpsertCurrency";
        public static string DeleteCurrency = "spCommon_DeleteCurrency";
        public static string DeleteCurrenciesByIds = "spCommon_DeleteCurrenciesByIds";
        public static string UpdateCurrenciesStatusByIds = "spCommon_UpdateCurrenciesStatusByIds";
        public static string UpdateTimeZonesStatusByIds = "spCommon_UpdateTimeZonesStatusByIds";
        public static string UpdateStatusByIds = "UpdateStatusByIds";
        public static string DeletesByIds = "DeletesByIds";

        public static string GetCompanies = "spMaster_GetCompanies";
        public static string GetCompanyById = "spMaster_GetCompanyById";
        public static string UpsertCompany = "spMaster_UpsertCompany";
        public static string DeleteCompany = "spMaster_DeleteCompany";
        public static string DeleteCompaniesByIds = "spMaster_DeleteCurrenciesByIds";
        public static string UpdateCompaniesStatusByIds = "spMaster_UpdateCurrenciesStatusByIds";

        public static string spProductsInsertUpdateDelete = "ProductsInsertUpdateDelete";
        public static string spMaster_GetProducts = "spMaster_GetProducts";
        public static string spMaster_GetProductById = "spMaster_GetProductById";

    }
}
