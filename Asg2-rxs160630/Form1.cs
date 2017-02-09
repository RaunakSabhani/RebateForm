/*=============================================================================
 |   Assignment:  CS6326 Assignment 2
 |       Author:  Raunak Sabhani 
 |     Language:  C#
 |    File Name:  Form1.cs
 |
 +-----------------------------------------------------------------------------
 |
 |  Description:  A data entry application for a Rebate Form.
 |
 |        Input:  The program takes in the First Name, Last Name, Middle Initials, Address[Line1, Line2, City, State, ZipCode], Phone Number,
 |                Email-ID, Proof-of-Purchase[Y/N] and Date Received of a customer as inputs.
 |
 |       Output:  The program stores the record in a file and allows the user to update, modify and delete records.
 |
 |       File Purpose: Main file containing the form class
 *===========================================================================*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asg2_rxs160630
{
    public partial class RebateForm : Form
    {
        FileHandler fh;
        int selectedIndex;
        string startTime = string.Empty;
        List<Person> personList = new List<Person>();
        private ListViewColumnSorter lvwColumnSorter;

        // Constructor for form
        public RebateForm()
        {
            InitializeComponent();
            fh = new FileHandler();

            this.CenterToScreen();
            //Assign the sorter
            lvwColumnSorter = new ListViewColumnSorter();
            this.ctlListView.ListViewItemSorter = lvwColumnSorter;

            ctlDateAttached.Value = DateTime.Today.Date;

            ctlModifyButton.Enabled = false;
            ctlDeleteButton.Enabled = false;
        }

        //Function called when a row in the list view is selected.
        private void ctlListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ctlListView.SelectedItems.Count > 0)
            {

                selectedIndex = 0;                
                String fullName = ctlListView.SelectedItems[0].Text;
                string[] eachName = fullName.Split(' ');

                //Load controls with selected record
                foreach(Person current in personList)
                {
                    if((String.Compare(current.firstName, eachName[0])==0) && (String.Compare(current.MI, eachName[1])==0) && (String.Compare(current.lastName, eachName[2])==0))
                    {
                        //Populate user details
                        showStatus("User populated");
                        ctlFirstName.Text = current.firstName;
                        ctlMiddleInitial.Text = current.MI;
                        ctlLastName.Text = current.lastName;
                        ctlAddressLine1.Text = current.addressLine1;
                        ctlAddressLine2.Text = current.addressLine2;
                        ctlCity.Text = current.city;
                        ctlState.Text = current.state;
                        ctlZip.Text = current.zip;
                        ctlPhone.Text = current.phoneNo;
                        ctlEmail.Text = current.email;
                        if (String.Compare(current.proof,"Yes")==0)
                        {
                            ctlRadioYes.Checked = true;
                        } else if(String.Compare(current.proof, "No") == 0)
                        {
                            ctlRadioNo.Checked = true;
                        }
                        ctlDateAttached.Value = Convert.ToDateTime(current.dateAttached);
                        break;
                    }
                    selectedIndex++;
                }
            }

            ctlModifyButton.Enabled = true;
            ctlDeleteButton.Enabled = true;
        }

        // Function called on form load
        private void Form1_Load(object sender, EventArgs e)
        {
            showStatus("Load users");
            //Read records from file and load list view
            foreach(string data in fh.readFileData())
            {
                string[] dataArray = data.Split('\t');
                Person current = new Person(dataArray[0],dataArray[2],dataArray[1], dataArray[3], dataArray[4], dataArray[5], dataArray[6], dataArray[7], dataArray
                    [8], dataArray[9], dataArray[10], dataArray[11]);
                current.startTime = dataArray[12];
                current.saveTime = dataArray[13];
                personList.Add(current);
            }
            loadListView(personList);
        }

        // Function to load list view
        private void loadListView(List<Person> personList)
        {
            foreach (Person current in personList)
            {
                ListViewItem lvi = new ListViewItem(current.firstName + " " + current.MI + " " + current.lastName);
                lvi.SubItems.Add(current.phoneNo);
                ctlListView.Items.Add(lvi);
            }
            showStatus("Users loaded");
        }

        //Function called when save button is clicked
        private void ctlSaveButton_Click(object sender, EventArgs e)
        {
            selectedIndex = -1;
            string data = String.Empty;
            string proof = String.Empty;
            if (ctlRadioYes.Checked)
            {
                proof = ctlRadioYes.Text;
            } else
            {
                proof = ctlRadioNo.Text;
            }

            if (checkValidations() == true)
            {
                //Create a new person object i.e. new entry
                Person newPerson = new Person(ctlFirstName.Text, ctlLastName.Text, ctlMiddleInitial.Text, ctlAddressLine1.Text, ctlAddressLine2.Text, ctlCity.Text, ctlState.Text, ctlZip.Text, ctlPhone.Text, ctlEmail.Text, proof, ctlDateAttached.Value.ToShortDateString());
                newPerson.startTime = startTime;
                newPerson.saveTime = DateTime.Now.ToString("HH:mm:ss");

                //Check if record can be inserted into file.
                if (!checkIfExists(newPerson.firstName, newPerson.lastName, newPerson.MI))
                {
                    showStatus("Saving record");
                    //Insert into file and reload list view
                    personList.Add(newPerson);
                    fh.save(personList);
                    showStatus("Record saved");

                    ctlListView.Items.Clear();
                    loadListView(personList);
                    
                    emptyControls();
                }
            }
        }

        //Function called when modify button clicked
        private void ctlModifyButton_Click(object sender, EventArgs e)
        {
            string data = String.Empty;
            string proof = String.Empty;
            if (ctlRadioYes.Checked)
            {
                proof = ctlRadioYes.Text;
            }
            else
            {
                proof = ctlRadioNo.Text;
            }
            if (checkValidations() == true)
            {
                //Modify the entry in list, write to file and reload list view
                Person newPerson = new Person(ctlFirstName.Text, ctlLastName.Text, ctlMiddleInitial.Text, ctlAddressLine1.Text, ctlAddressLine2.Text, ctlCity.Text, ctlState.Text, ctlZip.Text, ctlPhone.Text, ctlEmail.Text, proof, ctlDateAttached.Value.ToShortDateString());

                if (!checkIfExists(newPerson.firstName, newPerson.lastName, newPerson.MI))
                {
                    newPerson.startTime = personList[selectedIndex].startTime;
                    newPerson.saveTime = personList[selectedIndex].saveTime;
                    showStatus("Updating user");
                    personList[selectedIndex] = newPerson;
                    fh.save(personList);
                    showStatus("User updated");

                    ctlListView.Items.Clear();
                    loadListView(personList);
                    
                    emptyControls();
                }
            }
        }

        //Function called when delete button clicked
        private void ctlDeleteButton_Click(object sender, EventArgs e)
        {
            //Remove from list, write to file and load list view
            showStatus("Deleting record");
            personList.RemoveAt(selectedIndex);
            fh.save(personList);
            showStatus("Record deleted");

            ctlListView.Items.Clear();
            loadListView(personList);
            
            emptyControls();
        }

        //Check if a reord with same name and middle initial already exists
        private Boolean checkIfExists(String firstName, String lastName, String MI)
        {
            int index = 0;
            //Iterate over list and check for existence
            foreach(Person current in personList)
            {
                if ((String.Compare(current.firstName, firstName) == 0) && (String.Compare(current.MI, MI) == 0) && (String.Compare(current.lastName, lastName) == 0))
                {
                    if (index != selectedIndex)
                    {
                        MessageBox.Show("Duplicate Entry with same name", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                }
                index = index + 1;
            }
            return false;
        }

        //Function called when new button clicked
        private void ctlNewButton_Click(object sender, EventArgs e)
        {
            emptyControls();

            ctlListView.SelectedItems.Clear();
            ctlModifyButton.Enabled = false;
            ctlDeleteButton.Enabled = false;
        }

        //Clear all field entries
        private void emptyControls()
        {
            ctlFirstName.Text = String.Empty;
            ctlMiddleInitial.Text = String.Empty;
            ctlLastName.Text = String.Empty;
            ctlAddressLine1.Text = String.Empty;
            ctlAddressLine2.Text = String.Empty;
            ctlCity.Text = String.Empty;
            ctlState.Text = String.Empty;
            ctlZip.Text = String.Empty;
            ctlPhone.Text = String.Empty;
            ctlEmail.Text = String.Empty;
            ctlDateAttached.Value = DateTime.Today.Date;
            ctlRadioYes.Checked = false;
            ctlRadioNo.Checked = false;
        }

        //Record start time of entry
        private void ctlFirstName_Enter(object sender, EventArgs e)
        {
            showStatus("Adding a record");
            if(ctlModifyButton.Enabled==false)
            {
                startTime = DateTime.Now.ToString("HH:mm:ss");
            }
        }

        //Check all validations
        private Boolean checkValidations()
        {
            //Check required fields not empty
            foreach(TextBox curenttb in this.Controls.OfType<TextBox>().Where(x => x.CausesValidation == true))
            {
                if (String.IsNullOrEmpty(curenttb.Text))
                {
                    MessageBox.Show("Please enter all required fields", "Incomplete Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            //Validate first name 
            if (!Regex.IsMatch(ctlFirstName.Text, @"^[\p{L} \.\-]+$"))
            {
                MessageBox.Show("Please enter valid first name. First name contains only letters, spcaes, dots and dashes", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            //Validate last name
            if (!Regex.IsMatch(ctlLastName.Text, @"^[\p{L} \.\-]+$"))
            {
                MessageBox.Show("Please enter valid last name. Last name contains only letters, spaces, dots and dashes", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //Validate city
            if (!Regex.IsMatch(ctlCity.Text, @"^[a-zA-Z\- ]+$"))
            {
                MessageBox.Show("Please enter a valid city. City can only contain letters, spaces and dashes.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //Validate state
            if (!Regex.IsMatch(ctlState.Text, @"^[a-zA-Z\- ]+$"))
            {
                MessageBox.Show("Please enter a valid state. State can only contain letters, spaces and dashes.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //Validate phone
            if (!Regex.IsMatch(ctlPhone.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("Please enter valid phone no. Phone number can only contain numbers.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //Validate email
            if (!Regex.IsMatch(ctlEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                MessageBox.Show("Please enter valid email address. Ex: raunaksabhani@gmail.com", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        //Function called when column header clicked for sorting
        private void ctlListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.ctlListView.Sort();
        }

        //Function to show status
        private void showStatus(String message)
        {
            ctlStatusLabel.Text = message;
            ctlStatusBar.Refresh();
            ctlStatusBar.ForeColor = Color.Black;
        }
    }
}
