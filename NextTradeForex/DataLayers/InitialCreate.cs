using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayers
{
    public class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet ADD CONSTRAINT CK_tblBase_Wallet_Amount CHECK (Amount >= 0);");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet ADD CONSTRAINT CK_tblBase_Wallet_MaximumWithdrawAmountPerTransaction CHECK (MaximumWithdrawAmountPerTransaction BETWEEN 0 AND 50000000);");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet ADD CONSTRAINT CK_tblBase_Wallet_MaximumDepositAmountPerTransaction CHECK (MaximumDepositAmountPerTransaction BETWEEN 0 AND 50000000);");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet ADD CONSTRAINT CK_tblBase_Wallet_MaximumDepositAmountPerDay CHECK (MaximumDepositAmountPerDay BETWEEN 0 AND 50000000);");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet ADD CONSTRAINT CK_tblBase_Wallet_MinimumAmount CHECK (MinimumAmount >= 0);");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet ADD CONSTRAINT CK_tblBase_Wallet_MinimumAmountAlertInSite CHECK (MinimumAmountAlertInSite >= 0);");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet ADD CONSTRAINT CK_tblBase_Wallet_MinimumAmountAlertByEmail CHECK (MinimumAmountAlertByEmail >= 0);");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet ADD CONSTRAINT CK_tblBase_Wallet_MinimumAmountAlertBySms CHECK (MinimumAmountAlertBySms >= 0);");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet ADD CONSTRAINT CK_tblBase_Wallet_IbanNumber CHECK (LEN(IbanNumber) = 28 OR LEN(IbanNumber) = 0);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet DROP CONSTRAINT CK_tblBase_Wallet_Amount");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet DROP CONSTRAINT CK_tblBase_Wallet_MaximumWithdrawAmountPerTransaction");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet DROP CONSTRAINT CK_tblBase_Wallet_MaximumDepositAmountPerTransaction");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet DROP CONSTRAINT CK_tblBase_Wallet_MaximumDepositAmountPerDay");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet DROP CONSTRAINT CK_tblBase_Wallet_MinimumAmount");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet DROP CONSTRAINT CK_tblBase_Wallet_MinimumAmountAlertInSite");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet DROP CONSTRAINT CK_tblBase_Wallet_MinimumAmountAlertByEmail");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet DROP CONSTRAINT CK_tblBase_Wallet_MinimumAmountAlertBySms");
            //migrationBuilder.Sql("ALTER TABLE tblBase_Wallet DROP CONSTRAINT CK_tblBase_Wallet_IbanNumber");
        }
    }
}
