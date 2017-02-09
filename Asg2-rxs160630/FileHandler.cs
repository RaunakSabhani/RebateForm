/*=============================================================================
 |   Assignment:  CS6326 Assignment 2
 |       Author:  Raunak Sabhani 
 |     Language:  C#
 |    File Name:  FileHandler.cs
 |
 +-----------------------------------------------------------------------------
 |
 |       File Purpose: File contains all the file operations
 *===========================================================================*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asg2_rxs160630
{
    //File handler for the application
    class FileHandler
    {
        public const string fileName = "CS6326Asg2.txt";

        //Read from file
        public List<String> readFileData()
        {
            if (File.Exists(fileName))
            {
                String[] data = File.ReadAllLines(fileName);
                return new List<String>(data);
            }
            return new List<String>();
        }


        //Save record. Write to file.
        public void save(List<Person> personList)
        {
            //Generate records
            List<String> dataList = new List<String>();
            foreach(Person current in personList)
            {
                String data = String.Empty;
                data = data + current.firstName+"\t";
                data = data + current.MI + "\t";
                data = data + current.lastName + "\t";
                data = data + current.addressLine1 + "\t";
                data = data + current.addressLine2 + "\t";
                data = data + current.city + "\t";
                data = data + current.state + "\t";
                data = data + current.zip + "\t";
                data = data + current.phoneNo + "\t";
                data = data + current.email + "\t";
                data = data + current.proof + "\t";
                data = data + current.dateAttached + "\t";
                data = data + current.startTime + "\t";
                data = data + current.saveTime;
                dataList.Add(data);
            }

            File.WriteAllLines(fileName, dataList);
        }
    }
}
