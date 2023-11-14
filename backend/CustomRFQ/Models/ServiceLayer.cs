using System.Text.Json;

namespace CustomRFQ.Models
{
    public class ServiceLayer
    {
        public class Login
        {
            public string SessionId { get; set; }
            public string Version { get; set; }
            public int SessionTimeout { get; set; }
        }

        public class Erro
        {
            public class Error
            {
                public int code { get; set; }
                public Message message { get; set; }
            }

            public class Message
            {
                public string lang { get; set; }
                public string value { get; set; }
            }

            public Error error { get; set; }

        }

        public class UserTablesMD
        {
            public class Root
            {
                public List<Value> value { get; set; }
            }

            public class Value
            {
                public string TableName { get; set; }
                public string TableDescription { get; set; }
                public string TableType { get; set; }
            }
        }

        public class UserFieldsMD
        {
            public class Root
            {
                public List<Value> value { get; set; }
            }

            public class ValidValuesMD
            {
                public string Value { get; set; }

                public string Description { get; set; }
            }

            public class Value
            {
                public string Name { get; set; }
                public string Type { get; set; }
                public int EditSize { get; set; }
                public string Description { get; set; }
                public string SubType { get; set; }
                public string DefaultValue { get; set; }
                public string TableName { get; set; }
                public string Mandatory { get; set; }
                public List<ValidValuesMD> ValidValuesMD { get; set; }
            }
        }

        public class MarketingDocument
        {
            public class Root
            {
                public List<Value> value { get; set; }
            }

            public class DocumentLine
            {
                public int VisualOrder { get; set; }
                public int LineNum { get; set; }
                public string ItemDescription { get; set; }
                public string ItemCode { get; set; }
                public string FreeText { get; set; }
                public double Quantity { get; set; }
                public double UnitPrice { get; set; }
            }

            public class Value
            {
                public int DocEntry { get; set; }
                public int DocNum { get; set; }
                public DateTime DocDate { get; set; }
                public string CardName { get; set; }
                public string CardCode { get; set; }
                public string DocCurrency { get; set; }
                public int SalesPersonCode { get; set; }
                public List<DocumentLine> DocumentLines { get; set; }
            }
        }

        public class BusinessPartner
        {
            public class ContactEmployee
            {
                public string FirstName { get; set; }
                public string E_Mail { get; set; }
                public string U_RSD_CustomRFQ { get; set; } = "N";
            }

            public class Value
            {
                public string CardCode { get; set; }
                public List<ContactEmployee> ContactEmployees { get; set; }
            }


        }

        public class SalesPersons
        {
            public class Value 
            {
                public string SalesEmployeeName { get; set; }
                public object Email { get; set; }
            }
        }

        public class Messages
        {
            public class MessageDataColumn
            {
                public string ColumnName { get; set; }
                public string Link { get; set; }
                public List<MessageDataLine> MessageDataLines { get; set; }
            }

            public class MessageDataLine
            {
                public string Object { get; set; }
                public string ObjectKey { get; set; }
                public string Value { get; set; }
            }

            public class RecipientCollection
            {
                public string SendInternal { get; set; }
                public string UserCode { get; set; }
            }

            public class Root
            {
                public Root()
                {
                    Subject = "Cotação Online - Atualizada";
                    Text = "A seguinte Cotação Online foi atualizada pelo fornecedor.";

                    MessageDataColumns = new()
                    {
                        new MessageDataColumn
                        {
                            ColumnName = "Documento",
                            Link = "tYES",
                            MessageDataLines = new()
                            {
                                new MessageDataLine
                                {
                                    Object = "540000006"
                                }
                            }
                        }
                    };

                    RecipientCollection = new()
                    {
                        new RecipientCollection
                        {
                            SendInternal = "tYES"
                        }
                    };
                }

                public List<MessageDataColumn> MessageDataColumns { get; set; }
                public List<RecipientCollection> RecipientCollection { get; set; }
                public string Subject { get; set; }
                public string Text { get; set; }

                public void SendAlert(string userCode, string docEntry, string docNum, Database.Config sL)
                {
                    this.MessageDataColumns[0].MessageDataLines[0].ObjectKey = docEntry;
                    this.MessageDataColumns[0].MessageDataLines[0].Value = docNum;
                    this.RecipientCollection[0].UserCode = userCode;

                    sL.SLApi.Post("Messages", JsonSerializer.Serialize(this)).Wait();
                }
            }
        }
    }
}
