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
//            var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJ0eElkIjoiYmJkM2E4ZGMtNTc1ZS00ZmViLTkxNTctMDAxYjU3M2MxMjhmIiwib3BlcmF0aW9uIjoiVFJBTlNGRVIiLCJzb3VyY2VUeXBlIjoiVkFVTFQiLCJzb3VyY2VJZCI6IjIiLCJkZXN0VHlwZSI6Ik9ORV9USU1FX0FERFJFU1MiLCJkZXN0SWQiOiIiLCJhc3NldCI6IkRPR0UiLCJhbW91bnQiOjg4LjIyNjM5NzY1LCJhbW91bnRTdHIiOiI4OC4yMjYzOTc2NSIsInJlcXVlc3RlZEFtb3VudCI6ODguMjI2Mzk3NjUsInJlcXVlc3RlZEFtb3VudFN0ciI6Ijg4LjIyNjM5NzY1IiwiZmVlIjoiMS4wMDAwMDAwMCIsImRlc3RBZGRyZXNzVHlwZSI6Ik9ORV9USU1FIiwiZGVzdEFkZHJlc3MiOiJEUGFFa1FXdlpBUG14bnpQWXRzRmljc2dXTXZQVWZOeHZjIiwiZGVzdGluYXRpb25zIjpbeyJhbW91bnROYXRpdmUiOjg4LjIyNjM5NzY1LCJhbW91bnROYXRpdmVTdHIiOiI4OC4yMjYzOTc2NSIsImFtb3VudFVTRCI6MTIuNzQzMDg3MTUsImRzdEFkZHJlc3MiOiJEUGFFa1FXdlpBUG14bnpQWXRzRmljc2dXTXZQVWZOeHZjIiwiZHN0QWRkcmVzc1R5cGUiOiJPTkVfVElNRSIsImRzdElkIjoiIiwiZHN0VHlwZSI6Ik9ORV9USU1FX0FERFJFU1MiLCJkaXNwbGF5RHN0QWRkcmVzcyI6IkRQYUVrUVd2WkFQbXhuelBZdHNGaWNzZ1dNdlBVZk54dmMiLCJhY3Rpb24iOiJBTExPVyIsImFjdGlvbkluZm8iOnsiY2FwdHVyZWRSdWxlTnVtIjoxMCwicnVsZXNTbmFwc2hvdElkIjo0ODkxLCJieUdsb2JhbFBvbGljeSI6ZmFsc2UsImJ5UnVsZSI6dHJ1ZSwiY2FwdHVyZWRSdWxlIjoie1widHlwZVwiOlwiVFJBTlNGRVJcIixcInRyYW5zYWN0aW9uVHlwZVwiOlwiVFJBTlNGRVJcIixcImFzc2V0XCI6XCIqXCIsXCJhbW91bnRcIjpcIjBcIixcIm9wZXJhdG9yc1wiOntcInVzZXJzXCI6W1wiNzc5MzI4YzUtZjVhYy04YjE4LThiMTYtMDIyYjE5NDE1MzExXCJdfSxcImFjdGlvblwiOlwiQUxMT1dcIixcInNyY1wiOntcImlkc1wiOltbXCIyXCIsXCJWQVVMVFwiLFwiKlwiXSxbXCI1XCIsXCJWQVVMVFwiLFwiKlwiXV19LFwiZHN0XCI6e1wiaWRzXCI6W1tcIipcIixcIlVOTUFOQUdFRFwiLFwiRVhURVJOQUxcIl1dfSxcImRzdEFkZHJlc3NUeXBlXCI6XCIqXCIsXCJhbW91bnRDdXJyZW5jeVwiOlwiVVNEXCIsXCJhbW91bnRTY29wZVwiOlwiU0lOR0xFX1RYXCIsXCJwZXJpb2RTZWNcIjowfSJ9fV0sImV4dGVybmFsVHhJZCI6ImZpcmVfdHhfMTdfMCIsInBsYXllcnMiOlsiMjE5MjZlY2MtNGE4YS00NjE0LWJiYWMtN2M1OTFhYTdlZmRkIiwiMjc5MDA3MzctNDZmNi00MDk3LWExNjktZDBmZjQ1NjQ5ZWQ1IiwiMzU2ZDFmYjItOTg1Yy00MmMwLTg1MDItZWQxNzUwZTI3MzFhIl0sInJlcXVlc3RJZCI6ImJiZDNhOGRjLTU3NWUtNGZlYi05MTU3LTAwMWI1NzNjMTI4ZiJ9.XSRD-pIdReIe9ubPApJ2f7k10BZy7lMXi5eJIwEtc-hBKXEW5u_yCY5tdQ5MqqkAa4I69xA42DjDisgYPM4l4qla7t-P6vyI_H1r776OXzdIHDjBmA2l_GgOw2SG-4PE-FpIqbROxdQFfnPEyWYcvD8h-7adqwUPbXlwK0afzTwgpIRa2W_tamtgiPgiILNBgBwlfzW5J1A_joX_z5uDkROCXKWf78OXXD-_1WwYyQHqxMnwTFg_jza1U0anK3trAxGlzQquQBlSgvMiJN_04Q3rYytqr28HCByi6TVZTPPQzTNn1KmAU-L-8ZsaPRITdxK0-HdGI65pwlRQJ_vPng";

//            var coSignerpublicKey = (await File.ReadAllTextAsync(@"D:\fireblocks uat\cosigner\cosigner-pub.pem"))
//                //.Replace("-----BEGIN RSA PRIVATE KEY-----", "")
//                //.Replace("-----END RSA PRIVATE KEY-----", "")
//                .Replace("-----BEGIN PUBLIC KEY-----", "")
//                .Replace("-----END PUBLIC KEY-----", "")
//                .Replace("\n", "")
//                .Replace("\r", "");

//            var privateKey = (await File.ReadAllTextAsync(@"D:\fireblocks uat\cosigner\private-new.pem"))
//                .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
//                .Replace("-----END RSA PRIVATE KEY-----", "")
//                .Replace("-----BEGIN PUBLIC KEY-----", "")
//                .Replace("-----END PUBLIC KEY-----", "")
//                .Replace("\n", "")
//                .Replace("\r", "");

//            var pubKey = (await File.ReadAllTextAsync(@"D:\fireblocks uat\cosigner\public-new.pem"))
//                .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
//                .Replace("-----END RSA PRIVATE KEY-----", "")
//                .Replace("-----BEGIN PUBLIC KEY-----", "")
//                .Replace("-----END PUBLIC KEY-----", "")
//                .Replace("\n", "")
//                .Replace("\r", "");

//            coSignerpublicKey = @"-----BEGIN PUBLIC KEY-----//wqeqweqweqweqweqweqwe123123412312321///..//-----END PUBLIC KEY-----".Replace("-----BEGIN PUBLIC KEY-----", "")
//                .Replace("-----END PUBLIC KEY-----", "")
//                .Replace("\n", "")
//                .Replace("\r", "");

//            var generator = new JwtTokenService(new KeyActivator(coSignerpublicKey, privateKey));
//            var request = generator.GetRequest(token);
//            Assert.IsTrue(generator.Validate(token));

//            var tokenGenerated = generator.Create(new CallbackResponse
//            {
//                Action = "APPROVE",
//                RejectionReason = "",
//                RequestId = request.RequestId
//            });

//            tokenGenerated = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJBY3Rpb24iOiJBUFBST1ZFIiwiUmVxdWVzdElkIjoiZTgwN2VlMTUtZjQxZC00NjRiLWI5N2UtMWUyMWEwYjg1ZjIwIiwiUmVqZWN0aW9uUmVhc29uIjoiIn0.bQrvbS3piMXggv-vh2Av_j3m63h7g2Pt4mybzc6xm3kHMHifhRXCT7m_4h1Sb_NjjyMmamW90AS5T_8d7j0Sw5zliAXl3lwVZmxxucsRf_aLjd0HBQbU7mbvJsbuRVudZrjdlKm6wjdXO5pKSnRvNmYO7fTvX-pWCopnHFBazHACeqz1MA_Ogs_tFjV6-3FOmsaEkbqxSnxbOT-L57RtYOpeGik9FfeuuUpfosXz0SRqrxMlJn1MSW_aMpdrG5tB3prcrOjH1s03pGZ4xsHQGHW0G4RkygnDn3zRV9CSzOea1k_Fv0-0mktf_Kg8cKg7xGzydl7gi7AG_GlEwaVKoQ";
//            var res = generator.Validate(tokenGenerated, pubKey);
        }
    }
}
