package phase1;

import java.io.IOException;

import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.openqa.selenium.WebDriver;

public class MedicalHistory 
{
	public String addMedicalHistory() throws IOException, InterruptedException
	{
		String result="";
		Browser browser=new Browser();
		WebDriver firefox=browser.openBrowser();
		XSSFSheet sheet=browser.openFile("addMedicalHistory.xlsx");
		return result;
	}
	

}
