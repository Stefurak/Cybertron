using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.IO;
using System.Text;

public class GameSaveLoad: MonoBehaviour 
{ 
	// An encoding example can be found at 
	// http://www.eggheadcafe.com/articles/system.xml.xmlserialization.asp 
	// This uses the KISS method and cheats a little by using 
	// the examples from the web page since they are fully described 
	
	// Local private members 
	Rect _Save, _Load, _SaveMSG, _LoadMSG; 
	bool _ShouldSave, _ShouldLoad,_SwitchSave,_SwitchLoad; 
	string _FileLocation,_FileName;
	UserData myData;
    public string PlayerName, highScoreName; 
	string _data;
    public float highScore;
 
	// Set the local variables
	void Start () 
	{ 
		// Set rectangles for messages 
		_Save			=	new Rect(10,80,100,20); 
		_Load			=	new Rect(10,100,100,20); 
		_SaveMSG		=	new Rect(10,120,400,40); 
		_LoadMSG		=	new Rect(10,140,400,40); 
		
		// Location annd name to Save to
		_FileLocation	=	Application.dataPath; 
		_FileName		=	"Game Data/SaveData.xml"; 
		
		// Object to store information and serialize later
		myData = new UserData(); 
	} 
 
	void Update () {} 
	
	// Draw GUI elements on screen
    public void forcedLoad()
    {
        // Load UserData into myData 
        LoadXML();

        // Check if data has been loaded
        if (_data.ToString() != "")
        {
            // Cast to type (UserData) so returned object is converted to correct type 
            myData = (UserData)DeserializeObject(_data);

            // Set Player position to loaded data
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                PlayerName = myData._iUser.name;
                GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterLogic>().score = (int)myData._iUser.score;
                GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterLogic>().level = (int)myData._iUser.level;
                highScore = myData._iUser.highscore;
                highScoreName = myData._iUser.highscoreName;
            }
        } 
    }

    public void forcedSave()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            // Information to save
            myData._iUser.score = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterLogic>().score;
            myData._iUser.level = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterLogic>().level;
            if (myData._iUser.score > highScore)
            {
                myData._iUser.highscore = myData._iUser.score;
                myData._iUser.highscoreName = myData._iUser.name;
            }
        }
        else
        {
            myData._iUser.score = 0.0f;
            myData._iUser.level = 1.0f;
            myData._iUser.name = PlayerName;
        }
        // Create XML based on the UserData Object! 
        _data = SerializeObject(myData);

        // Write to file, resulting XML from  serialization process 
        CreateXML();

        // Print saves data
        Debug.Log(_data); 
    }
 
	/* The following metods came from the referenced URL */ 
	string UTF8ByteArrayToString(byte[] characters) 
	{      
		UTF8Encoding encoding = new UTF8Encoding(); 
		string constructedString = encoding.GetString(characters); 
		return (constructedString); 
	} 
	
	byte[] StringToUTF8ByteArray(string pXmlString) 
	{ 
		UTF8Encoding encoding = new UTF8Encoding(); 
		byte[] byteArray = encoding.GetBytes(pXmlString); 
		return byteArray; 
	} 
	
	// Serialize UserData object of myData 
	string SerializeObject(object pObject) 
	{ 
		string XmlizedString = null; 
		MemoryStream memoryStream = new MemoryStream(); 
		XmlSerializer xs = new XmlSerializer(typeof(UserData)); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		xs.Serialize(xmlTextWriter, pObject); 
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream; 
		XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray()); 
		return XmlizedString; 
	} 
 
	// Deserialize it back into its original form 
	object DeserializeObject(string pXmlizedString) 
	{ 
		XmlSerializer xs = new XmlSerializer(typeof(UserData)); 
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
		//XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		return xs.Deserialize(memoryStream); 
	}
	
	// Save XML Method
	void CreateXML() 
	{ 
		// Create writer so file can be written to
		StreamWriter writer; 
		
		// Load file info
		FileInfo t = new FileInfo(_FileLocation+"\\"+ _FileName); 
    	
		// Check if file exists
		if(!t.Exists) 
		{ 
			// Creates or opens a file for writing UTF-8 encoded text
			writer = t.CreateText(); 
		} 
		else 
		{
			// Delete old file
			t.Delete();
			// Creates or opens a file for writing UTF-8 encoded text.
			writer = t.CreateText(); 
		} 
		
        // Encrypt data to write to file
		//_data = Encrypt(_data);

        // Write information to file
		writer.Write(_data); 
		writer.Close(); 
		
		Debug.Log("File written.");
	} 
	
	// Load XML File
	void LoadXML() 
	{ 
		// Create reader so file can be read
		StreamReader r = File.OpenText(_FileLocation+"\\"+ _FileName); 
		
		// Read information from opened file
		string _info = r.ReadToEnd(); 
		
		// Decrypt after loading
		//_info = Decrypt(_info);
		
		// Close file
		r.Close(); 
		
		// Store information to parse
		_data = _info; 
		
		Debug.Log("File Read"); 
	}
	
	public static string Encrypt (string toEncrypt)
	{
		// Encode a set of characters from specified String into the specified byte array
		// 256-AES key, specified with block and key sizes that may be any multiple of 32 bits
		byte[] keyArray = UTF8Encoding.UTF8.GetBytes ("12345678901234567890123456789012");
				
		// What needs to be converted is stored in the byte[]
		byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes (toEncrypt);
	
		// Create encruption class
		RijndaelManaged rDel = new RijndaelManaged ();
		
		// Assign key generated above
		rDel.Key = keyArray;
		
		// http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
		rDel.Mode = CipherMode.ECB;
		
		// Better lang support
		// Specifies type of padding to apply when the message data block is shorter than the full number of bytes needed for a cryptographic operation
		// http://msdn.microsoft.com/en-us/library/system.security.cryptography.paddingmode.aspx
		rDel.Padding = PaddingMode.PKCS7;
		
		// Create encryptor based on key above
		// http://msdn.microsoft.com/en-us/library/system.security.cryptography.rijndaelmanaged.createencryptor.aspx
		ICryptoTransform cTransform = rDel.CreateEncryptor ();
		
		// Transforms the specified region of the specified byte array.
		// http://msdn.microsoft.com/en-us/library/system.security.cryptography.icryptotransform.transformfinalblock.aspx
		byte[] resultArray = cTransform.TransformFinalBlock (toEncryptArray, 0, toEncryptArray.Length);
		
		// Return created string
		return Convert.ToBase64String (resultArray, 0, resultArray.Length);
	}

	public static string Decrypt (string toDecrypt)
	{
		// Encode a set of characters from specified String into the specified byte array
		// 256-AES key, specified with block and key sizes that may be any multiple of 32 bits
		byte[] keyArray = UTF8Encoding.UTF8.GetBytes ("12345678901234567890123456789012");
	
		// What needs to be converted is stored in the byte[]
		byte[] toEncryptArray = Convert.FromBase64String (toDecrypt);
		
		// Create encruption class
		RijndaelManaged rDel = new RijndaelManaged ();
		
		// Assign key generated above
		rDel.Key = keyArray;
		
		// http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
		rDel.Mode = CipherMode.ECB;
		
		// Better lang support
		// Specifies type of padding to apply when message data block is shorter than the full number of bytes needed for a cryptographic operation
		rDel.Padding = PaddingMode.PKCS7;
		
		// Create decryptor based on key above
		ICryptoTransform cTransform = rDel.CreateDecryptor ();
		
		// Transforms the specified region of the specified byte array.
		byte[] resultArray = cTransform.TransformFinalBlock (toEncryptArray, 0, toEncryptArray.Length);
		
		// Return created string
		return UTF8Encoding.UTF8.GetString (resultArray);
	}
	
	
} 
 
// UserData is a custom Class that holds the information to store in XML format
public class UserData 
{ 
	// Define a default instance of the structure 
	public DemoData _iUser; 
	
	// Default constructor doesn't really do anything at the moment 
	public UserData() { } 
	
	// Anything to store in XML file is defined here 
	public struct DemoData 
	{ 
		public float score; 
		public float level;
        public string name;
        public float highscore;
        public string highscoreName;
	} 
}


