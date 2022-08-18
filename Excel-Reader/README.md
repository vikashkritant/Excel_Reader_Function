# Excel_Reader_Function
Sample code to trigger azure function on blob upload and read that

Overview:
This project has a azure function which will get triggered when any file gets uploaded to azure storage for which the 
connection string is defined in local.settings.json file in key named StorageConnection.

There is also a sample excel file is provided. You can just upload it to the container exceldata in your azure storage
and then this function will get triggered and print the name at console.
