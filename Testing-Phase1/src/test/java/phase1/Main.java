package phase1;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;

public class Main
{
	public static void main(String[]args) throws InterruptedException, IOException
	{
		// start test login
		String login=Account.Login();
		
		Files.write(Paths.get("loginResult.txt"), login.getBytes(), StandardOpenOption.CREATE);
		//  end test login
		
		// start test signUp
		/*String signUp=Account.SignUp();
		Files.write(Paths.get("signUpResult.txt"), signUp.getBytes(), StandardOpenOption.CREATE);
		// end signUp
		
		// start Ask For Support
		String askForSupport=Account.AskForSupport();
		Files.write(Paths.get("askResult.txt"), askForSupport.getBytes(), StandardOpenOption.CREATE);
		// end Ask For Support*/
	}

}
