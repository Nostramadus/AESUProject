Welcome to AES-Utility,

You find the full program on github: https://github.com/Nostramadus/AESUProject

this is a C# service that allows you to encrypt and decrypt messages according to AES.
To use this program you have execute it from the windows commandline.
Step 1a: Open a command prompt (In Windows 7: open the Start Menu and type "cmd") and navigate to the folder:
	[...]\AESU\bin\Debug  once you navigate here you can proceed with Step 2
Step 1b: Shift+Right click on the "Debug"-folder in your Windows-Explorer and click on "Open Command Window here"

Step 2: type in the command specified on what you want the program to do:
	AESU -e[d] -k key <-i initial_vector> -m CBC  -in input_file -out output_file
	AESU: is the name of the program you are calling (always the same)
	-e[d]: use either -e for encryptiond or -d for decryption
	-k: REQUIRED
	key: here you are entering a key with 16 or 24 or 32 characters (this is they key used for encryption and decryption
	     if you encrypt a message with a key, make sure you use the same key for decryption!)
	-i: is optional when the mode "ecb" is entered, but REQUIRED for the other modes
	initial_vector: here you enter the initial Vector, it has to have the same length as the key
			(same as above, make sure you use the same initial Vector for encryption and decryption)
	-m: REQUIRED
	CBC: here you can enter: ecb, cbc, cfb, ofb or ctr (NOTHING else, ONLY THIS writing no capital letters!)
	     (make sure you use the same mode for encryption and decryption)
	-in: REQUIRED
	input_file: enter the path to your document that you want to be encrypted (prefered .txt documents, other documents may not work!)
		    (e.g. plaintext.txt when your document is in the "Debug"-folder)
	-out: REQUIRED
	output_file: enter the path to your document where the decrypted/encrypted document should be saved to (prefered .txt documents, other documents may not work!)
		     (e.g. ciphertext.txt when your document should be saved to the "Debug"-folder)

Information: They key does not allow spaces, use "_" instead.
Information: This Program is designed to work in Windows 7, in other Operation Systems there might ocure errors.

Enjoy using the AES-Utility brought to you by Sven Graesser
