﻿using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace Kanbean_Project
{
    public partial class registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }

        protected void passwordCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int isNum = args.Value.IndexOfAny("1234567890".ToCharArray());
            int isLetterSmall = args.Value.IndexOfAny("qwertyuiopåäölkjhgfdsazxcvbnm".ToCharArray());
            int isLetterBig = args.Value.IndexOfAny("QWERTYUIOPÅÄÖLKJHGFDSAZXCVBNM".ToCharArray());
            bool isLongEnough = args.Value.Length > 5;
            if ((isNum > -1) && (isLetterSmall > -1) && (isLetterBig > -1) && isLongEnough)
                args.IsValid = true;
            else
                args.IsValid = false;
        }

        protected void btnRegiter_Click(object sender, EventArgs e)
        {
            if (this.IsValid)
            {

                OleDbConnection myConnection = new OleDbConnection();
                myConnection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|LanbanDatabase.mdb;";
                myConnection.Open();

                OleDbCommand myCommand = new OleDbCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = "SELECT * FROM [User] WHERE [Username] = @Username OR [Email] = @Email";
                myCommand.Parameters.AddWithValue("@Username", usernameTextBox.Text);
                myCommand.Parameters.AddWithValue("@Email", emailTextBox.Text);
                try
                {
                    if (myCommand.ExecuteScalar().ToString() != "")
                    {
                        resultLabel.Text = "This username or email is used!";
                        btnOK.Visible = false;
                        btnCancel.Visible = true;
                        registerFormPopup.Show();
                    }
                }
                catch
                {
                    myCommand.CommandText = "INSERT INTO [User]([Username], [Password], [Email], [Level]) VALUES ( @Username, @Password, @Email, 2)";
                    myCommand.Parameters.AddWithValue("@Username", usernameTextBox.Text);
                    myCommand.Parameters.AddWithValue("@Password", passwordTextBox.Text);
                    myCommand.Parameters.AddWithValue("@Email", emailTextBox.Text);
                    myCommand.ExecuteNonQuery();
                    myConnection.Close();
                    resultLabel.Text = "Your account is created! Click OK to login.";
                    btnOK.Visible = true;
                    btnCancel.Visible = false;
                    registerFormPopup.Show();
                }
            }
            else
            {
                resultLabel.Text = "Invalid information! Check information again.";
                btnOK.Visible = false;
                btnCancel.Visible = true;
                registerFormPopup.Show();
            }

        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx");
        }
    }
}

