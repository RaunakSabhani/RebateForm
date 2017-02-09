/*=============================================================================
 |   Assignment:  CS6326 Assignment 2
 |       Author:  Raunak Sabhani 
 |     Language:  C#
 |    File Name:  Person.cs
 |
 +-----------------------------------------------------------------------------
 |
 |       File Purpose: Person class which contains each entry
 *===========================================================================*/

using System;

//Class which stores each entry
public class Person
{
    public String firstName;
    public String lastName;
    public String MI;
    public String addressLine1;
    public String addressLine2;
    public String city;
    public String state;
    public String zip;
    public String phoneNo;
    public String email;
    public String proof;
    public String dateAttached;
    public String startTime;
    public String saveTime;


    //Person class constructor
	public Person(String firstName, String lastName, String MI, String addressLine1, String addressLine2, String city,String state, String zip, String phoneNo, String email, String proof, String date)
	{
        this.firstName = firstName;
        this.lastName = lastName;
        this.MI = MI;
        this.addressLine1 = addressLine1;
        this.addressLine2 = addressLine2;
        this.city = city;
        this.state = state;
        this.zip = zip;
        this.phoneNo = phoneNo;
        this.email = email;
        this.proof = proof;
        this.dateAttached = date;
	}
}
