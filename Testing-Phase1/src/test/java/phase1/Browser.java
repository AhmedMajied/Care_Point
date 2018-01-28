package phase1;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;

import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.firefox.FirefoxDriver;

public class Browser 
{
	public WebDriver openBrowser() throws InterruptedException
	{
		System.setProperty("webdriver.gecko.driver","geckodriver.exe");
		WebDriver firefox=new FirefoxDriver();
		firefox.manage().window().maximize();
		firefox.get("http://localhost:51902/");
		Thread.sleep(3000);
		return firefox;
	}
	public XSSFSheet openFile(String fileName) throws IOException
	{
		FileInputStream fileInputStream=new FileInputStream(new File(fileName));
		XSSFWorkbook workBook=new XSSFWorkbook(fileInputStream);
		XSSFSheet sheet=workBook.getSheetAt(0);
		workBook.close();
		return sheet;
	}

}
