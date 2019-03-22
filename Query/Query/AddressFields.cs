using System;
using System.Collections.Generic;

namespace Query
{
    public static class AddressFields
    {
        public static int Create(Models.AddressField model)
        {
            return Sql.ExecuteScalar<int>(
                "AddressField_Create",
                new Dictionary<string, object>()
                {
                    {"userId", model.userId },
                    {"label", model.label },
                    {"datatype", model.datatype },
                    {"sort", model.sort }
                }
            );
        }

        public static void UpdateValue(int addressId, int fieldId, bool? bit = null, DateTime? date = null, int? number = null, string text = null)
        {
            Sql.ExecuteNonQuery(
                "AddressFieldValue_Update",
                new Dictionary<string, object>()
                {
                    {"addressId", addressId },
                    {"fieldId", fieldId },
                    {"bit", bit },
                    {"date", date },
                    {"number", number },
                    {"text", string.IsNullOrEmpty(text) ? null : text }
                }
            );
        }

        public static List<Models.AddressFieldValues>GetValues(int addressId)
        {
            return Sql.Populate<Models.AddressFieldValues>(
                "AddressFields_GetValues",
                new Dictionary<string, object>()
                {
                    {"addressId", addressId }
                }
            );
        }

        public static void Delete(int fieldId)
        {
            Sql.ExecuteNonQuery(
                "AddressField_Delete",
                new Dictionary<string, object>()
                {
                    {"fieldId", fieldId }
                }
            );
        }

        public static void UpdateLabel(int fieldId, string label)
        {
            Sql.ExecuteNonQuery(
                "AddressField_UpdateLabel",
                new Dictionary<string, object>()
                {
                    {"fieldId", fieldId },
                    {"label", label }
                }
            );
        }

        public static void UpdateSort(int fieldId, short sort)
        {
            Sql.ExecuteNonQuery(
                "AddressField_UpdateSort",
                new Dictionary<string, object>()
                {
                    {"fieldId", fieldId },
                    {"sort", sort }
                }
            );
        }
    }
}
