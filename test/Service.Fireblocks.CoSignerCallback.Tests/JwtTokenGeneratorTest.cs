using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Service.Fireblocks.CoSignerCallback.Domain;
using Service.Fireblocks.CoSignerCallback.Domain.Models;
using Service.Fireblocks.CoSignerCallback.Services;

namespace Service.Fireblocks.Webhook.Tests
{
    public class JwtTokenGeneratorTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            //var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJ0eElkIjoiMGU1MzVkOTQtZDkwNC00M2EzLTk0MGEtMTI1MWFiMDRkYjc0Iiwib3BlcmF0aW9uIjoiVFJBTlNGRVIiLCJzb3VyY2VUeXBlIjoiVkFVTFQiLCJzb3VyY2VJZCI6IjExIiwiZGVzdFR5cGUiOiJVTk1BTkFHRUQiLCJkZXN0SWQiOiI1NzdhYjA0OS1hNjk5LTU1MmItZjM4My03Y2NjZDFiNDVkODAiLCJhc3NldCI6IkVUSF9URVNUIiwiYW1vdW50IjowLjAwNDEwMDAwLCJhbW91bnRTdHIiOiIwLjAwNDEwMDAwMDAwMDAwMDAwMCIsInJlcXVlc3RlZEFtb3VudCI6MC4wMDQxMDAwMCwicmVxdWVzdGVkQW1vdW50U3RyIjoiMC4wMDQxIiwiZmVlIjoiMC4wMDA2NDA0OTA4NzUxNDMwMDAiLCJkZXN0QWRkcmVzc1R5cGUiOiJXSElURUxJU1RFRCIsImRlc3RBZGRyZXNzIjoiMHgxRWFiN2Q0MTJhMjVhNWQwMEVjM2QwNDY0OGFhNTRDZUE0YUI3ZTk0IiwiZGVzdGluYXRpb25zIjpbeyJhbW91bnROYXRpdmUiOjAuMDA0MTAwMDAsImFtb3VudE5hdGl2ZVN0ciI6IjAuMDA0MSIsImFtb3VudFVTRCI6MTMuMzU4NTk0MTYsImRzdEFkZHJlc3MiOiIweDFFYWI3ZDQxMmEyNWE1ZDAwRWMzZDA0NjQ4YWE1NENlQTRhQjdlOTQiLCJkc3RBZGRyZXNzVHlwZSI6IldISVRFTElTVEVEIiwiZHN0SWQiOiI1NzdhYjA0OS1hNjk5LTU1MmItZjM4My03Y2NjZDFiNDVkODAiLCJkc3ROYW1lIjoiMHgxRWFiN2Q0MTJhMjVhNWQwMEVjM2QwNDY0OGFhNTRDZUE0YUI3ZTk0IiwiZHN0U3ViVHlwZSI6IkVYVEVSTkFMIiwiZHN0VHlwZSI6IlVOTUFOQUdFRCIsImRpc3BsYXlEc3RBZGRyZXNzIjoiMHgxRWFiN2Q0MTJhMjVhNWQwMEVjM2QwNDY0OGFhNTRDZUE0YUI3ZTk0IiwiYWN0aW9uIjoiQUxMT1ciLCJhY3Rpb25JbmZvIjp7ImNhcHR1cmVkUnVsZU51bSI6MTEsInJ1bGVzU25hcHNob3RJZCI6NDc0MywiYnlHbG9iYWxQb2xpY3kiOmZhbHNlLCJieVJ1bGUiOnRydWUsImNhcHR1cmVkUnVsZSI6IntcInR5cGVcIjpcIlRSQU5TRkVSXCIsXCJ0cmFuc2FjdGlvblR5cGVcIjpcIlRSQU5TRkVSXCIsXCJhc3NldFwiOlwiKlwiLFwiYW1vdW50XCI6XCIwXCIsXCJvcGVyYXRvcnNcIjp7XCJ1c2Vyc1wiOltcIjJlMmY4ZDE2LTNiZWEtNTJmNS04NTM4LWNhYmRhMDVkNjU3Y1wiXX0sXCJhcHBseUZvckFwcHJvdmVcIjp0cnVlLFwiYWN0aW9uXCI6XCJBTExPV1wiLFwic3JjXCI6e1wiaWRzXCI6W1tcIjExXCIsXCJWQVVMVFwiLFwiKlwiXSxbXCIxMlwiLFwiVkFVTFRcIixcIipcIl1dfSxcImRzdFwiOntcImlkc1wiOltbXCIqXCIsXCJVTk1BTkFHRURcIixcIkVYVEVSTkFMXCJdXX0sXCJkc3RBZGRyZXNzVHlwZVwiOlwiKlwiLFwiYW1vdW50Q3VycmVuY3lcIjpcIlVTRFwiLFwiYW1vdW50U2NvcGVcIjpcIlNJTkdMRV9UWFwiLFwicGVyaW9kU2VjXCI6MH0ifX1dLCJleHRlcm5hbFR4SWQiOiJmaXJlX3R4XzYyMl8wIiwicmVxdWVzdElkIjoiMGU1MzVkOTQtZDkwNC00M2EzLTk0MGEtMTI1MWFiMDRkYjc0In0.fLTxPvgdV108fPe7MkJ55VIwJ1vnvmd3UTY_o-qLyY9c8YT-vjuidXqsdiUd6TBYyt7k7P0rHyrbtkHI6BBhEYLvhEnqEB_9YDB8ODZTc_F2_i2gV1BMBqZd9Fk2feK9DxL32MQ8fkEtl-_WhKszOuEX4tRxy52o4SMJQ8yYGiqKZC7qu7shCCt9qrq4SgHXRMkgN-YlFkKJ8nTTmc20iPaOrpL5jZwIPym8hlVDCZaKPs67KrHWKHCoJnCn1ozRe1IK3AShx8zhh-4tw2Ne_WvWIiy7hxp6ltVVXmPOHfZqkBrYXgnQjKDHnqRTvAqi91jX8xNaiMiFeBz9FmDZfQ";
            //var generator = new JwtTokenGenerator();

            //var privateKey = (await File.ReadAllTextAsync(@"D:\fireblocks uat\cosigner\public-new.pem"))
            //    //.Replace("-----BEGIN RSA PRIVATE KEY-----", "")
            //    //.Replace("-----END RSA PRIVATE KEY-----", "")
            //    .Replace("-----BEGIN PUBLIC KEY-----", "")
            //    .Replace("-----END PUBLIC KEY-----", "")
            //    .Replace("\n", "")
            //    .Replace("\r", "");

            //generator.ValidateJasonWebToken(privateKey, token);
        }

        [Test]
        public async Task Test2()
        {
            var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJ0eElkIjoiOTQ5YWRhMDMtMWZlNS00ZjU3LTk4ZGMtYjY4MzM5NDE1Y2UyIiwib3BlcmF0aW9uIjoiVFJBTlNGRVIiLCJzb3VyY2VUeXBlIjoiVkFVTFQiLCJzb3VyY2VJZCI6IjExIiwiZGVzdFR5cGUiOiJVTk1BTkFHRUQiLCJkZXN0SWQiOiI1NzdhYjA0OS1hNjk5LTU1MmItZjM4My03Y2NjZDFiNDVkODAiLCJhc3NldCI6IkVUSF9URVNUIiwiYW1vdW50IjowLjAwNDEwMDAwLCJhbW91bnRTdHIiOiIwLjAwNDEwMDAwMDAwMDAwMDAwMCIsInJlcXVlc3RlZEFtb3VudCI6MC4wMDQxMDAwMCwicmVxdWVzdGVkQW1vdW50U3RyIjoiMC4wMDQxIiwiZmVlIjoiMC4wMDEwNjM2MDczODQ4NDcwMDAiLCJkZXN0QWRkcmVzc1R5cGUiOiJXSElURUxJU1RFRCIsImRlc3RBZGRyZXNzIjoiMHgxRWFiN2Q0MTJhMjVhNWQwMEVjM2QwNDY0OGFhNTRDZUE0YUI3ZTk0IiwiZGVzdGluYXRpb25zIjpbeyJhbW91bnROYXRpdmUiOjAuMDA0MTAwMDAsImFtb3VudE5hdGl2ZVN0ciI6IjAuMDA0MSIsImFtb3VudFVTRCI6MTIuMzA4MzIwNTIsImRzdEFkZHJlc3MiOiIweDFFYWI3ZDQxMmEyNWE1ZDAwRWMzZDA0NjQ4YWE1NENlQTRhQjdlOTQiLCJkc3RBZGRyZXNzVHlwZSI6IldISVRFTElTVEVEIiwiZHN0SWQiOiI1NzdhYjA0OS1hNjk5LTU1MmItZjM4My03Y2NjZDFiNDVkODAiLCJkc3ROYW1lIjoiMHgxRWFiN2Q0MTJhMjVhNWQwMEVjM2QwNDY0OGFhNTRDZUE0YUI3ZTk0IiwiZHN0U3ViVHlwZSI6IkVYVEVSTkFMIiwiZHN0VHlwZSI6IlVOTUFOQUdFRCIsImRpc3BsYXlEc3RBZGRyZXNzIjoiMHgxRWFiN2Q0MTJhMjVhNWQwMEVjM2QwNDY0OGFhNTRDZUE0YUI3ZTk0IiwiYWN0aW9uIjoiQUxMT1ciLCJhY3Rpb25JbmZvIjp7ImNhcHR1cmVkUnVsZU51bSI6MTEsInJ1bGVzU25hcHNob3RJZCI6NDc0MywiYnlHbG9iYWxQb2xpY3kiOmZhbHNlLCJieVJ1bGUiOnRydWUsImNhcHR1cmVkUnVsZSI6IntcInR5cGVcIjpcIlRSQU5TRkVSXCIsXCJ0cmFuc2FjdGlvblR5cGVcIjpcIlRSQU5TRkVSXCIsXCJhc3NldFwiOlwiKlwiLFwiYW1vdW50XCI6XCIwXCIsXCJvcGVyYXRvcnNcIjp7XCJ1c2Vyc1wiOltcIjJlMmY4ZDE2LTNiZWEtNTJmNS04NTM4LWNhYmRhMDVkNjU3Y1wiXX0sXCJhcHBseUZvckFwcHJvdmVcIjp0cnVlLFwiYWN0aW9uXCI6XCJBTExPV1wiLFwic3JjXCI6e1wiaWRzXCI6W1tcIjExXCIsXCJWQVVMVFwiLFwiKlwiXSxbXCIxMlwiLFwiVkFVTFRcIixcIipcIl1dfSxcImRzdFwiOntcImlkc1wiOltbXCIqXCIsXCJVTk1BTkFHRURcIixcIkVYVEVSTkFMXCJdXX0sXCJkc3RBZGRyZXNzVHlwZVwiOlwiKlwiLFwiYW1vdW50Q3VycmVuY3lcIjpcIlVTRFwiLFwiYW1vdW50U2NvcGVcIjpcIlNJTkdMRV9UWFwiLFwicGVyaW9kU2VjXCI6MH0ifX1dLCJleHRlcm5hbFR4SWQiOiJmaXJlX3R4XzYzMV8wIiwicGxheWVycyI6WyIyMTkyNmVjYy00YThhLTQ2MTQtYmJhYy03YzU5MWFhN2VmZGQiLCIyNzkwMDczNy00NmY2LTQwOTctYTE2OS1kMGZmNDU2NDllZDUiLCIzMzRlOWEzNC04NGE0LTRjMDctODNhNC1mNjJiYWJlMjc1NzMiXSwicmVxdWVzdElkIjoiOTQ5YWRhMDMtMWZlNS00ZjU3LTk4ZGMtYjY4MzM5NDE1Y2UyIn0.bx_4goOvdNBdCgkWjdvcOfwR8Ez-mTcXoNKDyIqS55gPWPoV0oenjJHmS0Yo8I64E8Whx88wZ1MkVmDdhZckDPu3qa2yvGCl29O13NfZEPd2hyVjOndYYOKV4rSpj1GkxVk_R41My6EXlMyMtVu-JhXCwzidP9lJFANxzxWtCdEfEGGWRRZkviQNhGsndg-7J_QkEQDVgKYRmG9S2aYqZDuBj-ZKu7P5Ppwbt-KJdUzivo4BKht-CnRbLyrwiomjDbkjraBBnbNK8jMsiqJFUaB7rehmEqLIVHy9fnvAQo6J5bH-PcaclqdW19HEjgBFk1BRYxa6L5YD-72RIvRTeg";

            var coSignerpublicKey = (await File.ReadAllTextAsync(@"D:\fireblocks uat\cosigner\cosigner-pub.pem"))
                //.Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                //.Replace("-----END RSA PRIVATE KEY-----", "")
                .Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", "");

            var privateKey = (await File.ReadAllTextAsync(@"D:\fireblocks uat\cosigner\private-new.pem"))
                .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                .Replace("-----END RSA PRIVATE KEY-----", "")
                .Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", "");

            var pubKey = (await File.ReadAllTextAsync(@"D:\fireblocks uat\cosigner\public-new.pem"))
                .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                .Replace("-----END RSA PRIVATE KEY-----", "")
                .Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", "");

            var generator = new JwtTokenService(new KeyActivator(coSignerpublicKey, privateKey));
            var request = generator.GetRequest(token);
            Assert.IsTrue(generator.Validate(token));

            var tokenGenerated = generator.Create(new CallbackResponse
            {
                Action = "APPROVE",
                RejectionReason = "",
                RequestId = request.RequestId
            });

            var res = generator.Validate(tokenGenerated, pubKey);
        }
    }
}
