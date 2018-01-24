package phase1;

import java.io.IOException;
import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.openqa.selenium.By;
import org.openqa.selenium.NoSuchElementException;
import org.openqa.selenium.WebDriver;
import java.text.SimpleDateFormat;
import java.util.Date;
public class Account 
{
	public static String Login() throws InterruptedException, IOException
	{
		Browser browser=new Browser();
		WebDriver firefox=browser.openBrowser();
		String url=firefox.getCurrentUrl();
		XSSFSheet sheet=browser.openFile("login.xlsx");
		int numOfRows=sheet.getLastRowNum();
		String mail,password,state;
		String results="";
		for(int row=0;row<=numOfRows;row++)
		{
			Thread.sleep(1000);
			firefox.findElement(By.id("itext-mail-phone")).clear();
			firefox.findElement(By.id("ipasswd")).clear();
			
			mail=sheet.getRow(row).getCell(0).getStringCellValue();
			password=sheet.getRow(row).getCell(1).getStringCellValue();
			state=sheet.getRow(row).getCell(2).getStringCellValue();
			
			firefox.findElement(By.id("itext-mail-phone")).sendKeys(mail);
			firefox.findElement(By.id("ipasswd")).sendKeys(password);
			firefox.findElement(By.id("ibtn-login")).click();
			String currentUrl=firefox.getCurrentUrl();
			Thread.sleep(5000);
			
			if(!url.equals(currentUrl))
			{
				firefox.findElement(By.id("ilink-logout")).click();
				Thread.sleep(3000);
			}
			// pass means logged in to website
			if((url.equals(currentUrl)&&(state.equals("not")))||(!url.equals(currentUrl))&&(state.equals("pass")))
			{
				results+="test case # "+(row+1)+" passed\r\n";
			}
			else
			{
				results+="test case # "+(row+1)+" failed\r\n";
			}
		}
		firefox.quit();
		return results;
	}
	public static String SignUp() throws IOException, InterruptedException
	{
		Browser browser=new Browser();
		WebDriver firefox=browser.openBrowser();
		XSSFSheet sheet=browser.openFile("signup.xlsx");
		int numOfRows=sheet.getLastRowNum();
		
		SimpleDateFormat simpleFormat = new SimpleDateFormat("MM-dd-yyyy");
		String result="";
		Thread.sleep(2000);
		firefox.findElement(By.id("ibtn-sign-up")).click();
		String url=firefox.getCurrentUrl();

		for(int row=0;row<numOfRows;row++)
		{
			String name,userName,email,state,phone,gender,nationalID,password,confirmPassword,speciality,professionLicense;
			Date birthdate = null;
			Thread.sleep(3000);
			name=sheet.getRow(row).getCell(0).getStringCellValue();
			userName=sheet.getRow(row).getCell(1).getStringCellValue();
			email=sheet.getRow(row).getCell(2).getStringCellValue();
			gender=sheet.getRow(row).getCell(3).getStringCellValue();
			nationalID=sheet.getRow(row).getCell(4).getStringCellValue();
			phone=sheet.getRow(row).getCell(5).getStringCellValue();
			password=sheet.getRow(row).getCell(6).getStringCellValue();
			confirmPassword=sheet.getRow(row).getCell(7).getStringCellValue();
			birthdate=sheet.getRow(row).getCell(8).getDateCellValue();
			speciality=sheet.getRow(row).getCell(9).getStringCellValue();
			professionLicense=sheet.getRow(row).getCell(10).getStringCellValue();
			firefox.findElement(By.id("itext-full-name")).sendKeys(name);
			firefox.findElement(By.id("itext-usrname")).sendKeys(userName);
			firefox.findElement(By.id("itext-mail")).sendKeys(email);
			firefox.findElement(By.id("itext-phone")).sendKeys(phone);
			if(gender.equals("male"))
				firefox.findElement(By.id("iradio-gender-male")).click();
			else if(gender.equals("female"))
				firefox.findElement(By.id("iradio-gender-female")).click();
			else
				firefox.findElement(By.id("iradio-gender-other"));
			
			firefox.findElement(By.id("itext-passwd")).sendKeys(password);
			firefox.findElement(By.id("itext-passwd-confirm")).sendKeys(confirmPassword);
			firefox.findElement(By.id("itext-birthdate")).sendKeys(simpleFormat.format(birthdate));
			if(!nationalID.equals(""))
			{
				firefox.findElement(By.id("ifile-national-id")).sendKeys(nationalID);
			}
			if(speciality.equals("none"))
				firefox.findElement(By.id("iradio-specialty-none")).click();
			else 
			{
				if(!professionLicense.equals(""))
				{
					firefox.findElement(By.id("ifile-license")).sendKeys(professionLicense);
				}
				 if(speciality.equals("doctor"))
						firefox.findElement(By.id("iradio-specialty-doc")).click();
				else
						firefox.findElement(By.id("iradio-specialty-pharmacist")).click();
			}
			firefox.findElement(By.id("ibtn-sign-up")).click();		
			String currentUrl=firefox.getCurrentUrl();
			state=sheet.getRow(row).getCell(11).getStringCellValue();
			// pass means acount is created
			if((state.equals("pass")&&(!url.equals(currentUrl)))||(state.equals("not")&&(url.equals(currentUrl))))
			{
				result+="test case # "+(row+1)+" passed\r\n";
			}
			else 
			{
				result+="test case # "+(row+1)+" failed\r\n";
			}
			if(!url.equals(currentUrl))
			{
				try {
					firefox.findElement(By.id("ilink-logout")).click();
				}
				catch (NoSuchElementException e){}
				Thread.sleep(3000);
				try {
					firefox.findElement(By.id("ibtn-sign-up")).click();
				}
				catch (NoSuchElementException e){}
			}
			Thread.sleep(2000);
			firefox.findElement(By.id("itext-full-name")).clear();
			firefox.findElement(By.id("itext-usrname")).clear();
			firefox.findElement(By.id("itext-mail")).clear();
			firefox.findElement(By.id("itext-phone")).clear();
			firefox.findElement(By.id("itext-passwd")).clear();
			firefox.findElement(By.id("itext-passwd-confirm")).clear();
			firefox.findElement(By.id("itext-birthdate")).clear();
			firefox.findElement(By.id("ifile-national-id")).clear();
			firefox.findElement(By.id("ifile-license")).clear();
		}
		firefox.quit();
		return result;
	}
	public static String AskForSupport() throws IOException, InterruptedException
	{
		Browser browser=new Browser();
		WebDriver firefox=browser.openBrowser();
		XSSFSheet sheet=browser.openFile("askForSupport.xlsx");
		int numOfRows=sheet.getLastRowNum();
		String mail,subject,body,status;
		Thread.sleep(3000);
		String url=firefox.getCurrentUrl();
		String result="";
		for(int row=0;row<=numOfRows;row++)
		{
			firefox.findElement(By.id("itext-mail")).clear();
			firefox.findElement(By.id("itext-subj")).clear();
			firefox.findElement(By.id("itextarea-msg")).clear();
			mail=sheet.getRow(row).getCell(0).getStringCellValue();
			subject=sheet.getRow(row).getCell(1).getStringCellValue();
			body=sheet.getRow(row).getCell(2).getStringCellValue();
			status=sheet.getRow(row).getCell(3).getStringCellValue();
			firefox.findElement(By.id("itext-mail")).sendKeys(mail);
			firefox.findElement(By.id("itext-subj")).sendKeys(subject);
			firefox.findElement(By.id("itextarea-msg")).sendKeys(body);
			firefox.findElement(By.id("ibtn-contact")).click();
			Thread.sleep(2000);
			String currentUrl=firefox.getCurrentUrl();
			if((status.equals("pass")&&(!url.equals(currentUrl)))||(status.equals("not")&&(url.equals(currentUrl))))
			{
				result+="test case # "+(row+1)+" passed\r\n";
			}
			else 
			{
				result+="test case # "+(row+1)+" failed\r\n";
			}
		}
		firefox.quit();
		return result;
	}
}
