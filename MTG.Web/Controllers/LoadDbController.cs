using Dapper;
using MTG.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MTG.Controllers
{
    public class LoadDbController : Controller
    {
        // GET: LoadDb
        public ActionResult Index()
        {
            var codes = new List<string>
            {
  //               "UST",
  //"UNH",
  //"UGL",
  //"pWOS",
  //"pWOR",
  //"pWCQ",
  //"pSUS",
  //"pSUM",
  //"pREL",
  //"pPRO",
  //"pPRE",
  //"pPOD",
  //"pMPR",
  //"pMGD",
  //"pMEI",
  //"pLPA",
  //"pLGM",
  //"pJGP",
  //"pHHO",
  //"pWPN",
  //"pGTW",
  //"pGRU",
  //"pGPX",
  //"pFNM",
  //"pELP",
  //"pDRC",
  //"pCMP",
  //"pCEL",
  //"pARL",
  //"pALP",
  //"p2HG",
  //"p15A",
  //"PD3",
  //"PD2",
  //"H09",
  //"PTK",
  //"POR",
  //"PO2",
  //"PCA",
  //"PC2",
  //"HOP",
  //"VMA",
  //"MMA",
  //"MM3",
  //"MM2",
  //"MED",
  //"ME4",
  //"ME3",
  //"ME2",
  //"IMA",
  //"EMA",
  //"A25",
  //"MPS_AKH",
  //"MPS",
  //"EXP",
  //"E02",
  //"V17",
  //"V16",
  //"V15",
  //"V14",
  //"V13",
  //"V12",
  //"V11",
  //"V10",
  //"V09",
  //"DRB",
  //"EVG",
  //"DDT",
  //"DDS",
  //"DDR",
  //"DDQ",
  //"DDP",
  //"DDO",
  //"DDN",
  //"DDM",
  //"DDL",
  //"DDK",
  //"DDJ",
  //"DDI",
  //"DDH",
  //"DDG",
  //"DDF",
  //"DDE",
  //"DDD",
  //"DDC",
  //"DD3_JVC",
  //"DD3_GVL",
  //"DD3_EVG",
  //"DD3_DVD",
  //"DD2",
  //"CNS",
  //"CN2",
  //"CMD",
  //"CMA",
  //"CM1",
  //"C17",
  //"C16",
  //"C15",
  //"C14",
  //"C13",
  //"CEI",
  //"CED",
  //"E01"
  //"ARC",
  //"ZEN",
  //"XLN",
  //"WWK",
  //"WTH",
  //"W17",
  //"W16",
  //"VIS",
  //"VAN",
  //"USG",
  //"ULG",
  //"UDS",
  //"TSP",
  //"TSB",
  //"TPR",
  //"TOR",
  //"TMP",
  //"THS",
  //"STH",
  //"SOM",
  //"SOK",
  //"SOI",
  //"SHM",
  //"SCG",
  //"S99",
  //"S00",
  //"RTR",
  //"RQS",
  //"ROE",
  //"RIX",
  //"RAV",
  //"PLS",
  //"PLC",
  //"PCY",
  //"ORI",
  //"ONS",
  //"OGW",
  //"ODY",
  //"NPH",
  //"NMS",
  //"MRD",
  //"MOR",
  //"MMQ",
  //"MIR",
  //"MGB",
  //"MD1",
  //"MBS",
  //"M15",
  //"M14",
  //"M13",
  //"M12",
  //"M11",
  //"M10",
  //"LRW",
  //"LGN",
  //"LEG",
  //"LEB",
  //"LEA",
  //"KTK",
  //"KLD",
  //"JUD",
  //"JOU",
  //"ITP",
  //"ISD",
  //"INV",
  //"ICE",
  //"HOU",
  //"HML",
  //"GTC",
  //"GPT",
  //"FUT",
  //"FRF_UGIN",
  //"FRF",
  //"FEM",
  //"EXO",
  //"EVE",
  //"EMN",
  //"DTK",
  //"DST",
  //"DRK",
  //"DPA",
  //"DKM",
  //"DKA",
  //"DIS",
  //"DGM",
  //"CST",
  //"CSP",
  //"CP3",
  //"CP2",
  //"CP1",
  //"CON",
  //"CHR",
  //"CHK",
  //"BTD",
  //"BRB",
  //"BOK",
  //"BNG",
  //"BFZ",
  //"AVR",
  //"ATQ",
  //"ATH",
  //"ARN",
  //"ARB",
  //"APC",
  //"ALL",
  //"ALA",
  //"AKH",
  //"AER",
  //"_9ED",
  //"_8ED",
  //"_7ED",
  //"_6ED",
  //"_5ED",
  //"_5DN",
  //"_4ED",
  //"_3ED",
  //"_2ED",
  //"_10E"

            };
                

            codes.ForEach(c =>
                uploadCards(c)
            );

            return View();
        }

        void uploadCards (string code)
        {
            //IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            //var path = "$." + code + ".cards";
            ////var query = $@"
            DECLARE @Sets VARCHAR(MAX)
                SELECT @Sets =
                BulkColumn FROM OPENROWSET
                (BULK 'AllSets.json', DATA_SOURCE = 'MyAzureJson', SINGLE_BLOB) JSON;

            INSERT INTO[New_Sets]
                SELECT Code, [name], ReleaseDate, Border, [Type], [Block], OldCode
                FROM OPENJSON(@Sets, '{path}')
                WITH(
                    Code            nvarchar(50)    '$.code',
                    [Name]          nvarchar(50)    '$.name',
                    ReleaseDate     datetime        '$.releaseDate',
                    Border          nvarchar(50)    '$.border',
                    [Type]          nvarchar(50)    '$.type',
                    [Block]         nvarchar(50)    '$.block',
                    OldCode         nvarchar(50)    '$.oldCode'
                )";

            //var query = $@"
            //    DECLARE @Cards VARCHAR(MAX)
            //    SELECT @Cards = 
            //    BulkColumn FROM OPENROWSET
            //    (BULK 'AllSets.json', DATA_SOURCE = 'MyAzureJson', SINGLE_BLOB) JSON;

            //    INSERT INTO New_Cards
            //    SELECT [Set], ID, [Name], ManaCost, ConvertedManaCost, ColorIdentity, Colors, [Type], [Types], SubTypes, Rarity, [Text], Flavor, Artist, Number, [Power], Toughness
            //    FROM OPENJSON(@Cards, '{path}')
            //    WITH(
            //     [Set]				nvarchar(50)	'$.fake',
            //     ID					int				'$.multiverseid',
            //     [Name]				nvarchar(50)	'$.name',	
            //     ManaCost			nvarchar(50)	'$.manaCost',
            //     ConvertedManaCost	nvarchar(50)	'$.cmc',
            //     [colorIdentity]		nvarchar(MAX)	AS JSON,
            //     [colors]			nvarchar(MAX)	AS JSON,
            //     [Type]				nvarchar(50)	'$.type',
            //     [types]				nvarchar(MAX)	AS JSON,
            //     [subtypes]			nvarchar(MAX)	AS JSON,
            //     Rarity				nvarchar(50)	'$.rarity',
            //     [Text]				nvarchar(MAX)	'$.text',
            //     Flavor				nvarchar(MAX)	'$.flavor',
            //     Artist				nvarchar(50)	'$.artist',
            //     Number				nvarchar(50)	'$.number',
            //     [Power]				nvarchar(50)	'$.power',
            //     Toughness			nvarchar(50)	'$.toughness'
            //    )

            //    UPDATE New_Cards SET [Set] = '{code}' WHERE [SET] is null
            //";

            //db.Query<dynamic>(query);
        }
    }
}