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
                    {"teamId", model.teamId },
                    {"label", model.label },
                    {"datatype", model.datatype },
                    {"sort", model.sort }
                }
            );
        }

        public static void SetValue(int addressId, string label, bool? bit = null, DateTime? date = null, int? number = null, string text = null)
        {
            Sql.ExecuteNonQuery(
                "AddressField_SetValue",
                new Dictionary<string, object>()
                {
                    {"addressId", addressId },
                    {"label", label },
                    {"bitvalue", bit },
                    {"datevalue", date },
                    {"numbervalue", number },
                    {"textvalue", string.IsNullOrEmpty(text) ? null : text }
                }
            );
        }

        public static void SetValue(int addressId, int fieldId, bool? bit = null, DateTime? date = null, int? number = null, string text = null)
        {
            Sql.ExecuteNonQuery(
                "AddressField_SetValueById",
                new Dictionary<string, object>()
                {
                    {"addressId", addressId },
                    {"fieldId", fieldId },
                    {"bitvalue", bit },
                    {"datevalue", date },
                    {"numbervalue", number },
                    {"textvalue", string.IsNullOrEmpty(text) ? "" : text }
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

        public static void Delete(int teamId, int fieldId)
        {
            Sql.ExecuteNonQuery(
                "AddressField_Delete",
                new Dictionary<string, object>()
                {
                    {"teamId", teamId },
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

        public static bool Exists(int teamId, string label)
        {
            if(Sql.ExecuteScalar<int>("AddressField_Exists",
                new Dictionary<string, object>()
                {
                    {"teamId", teamId },
                    {"label", label }
                }) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
