package phase1;

import java.io.IOException;
import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.openqa.selenium.By;
import org.openqa.selenium.NoSuchElementException;
import org.openqa.selenium.WebDriver;
public class Account 
{
	public static String Login() throws InterruptedException, IOException
	{
		Browser browser=new Browser();
		WebDriver firefox=browser.openBrowser();
		XSSFSheet sheet=browser.openFile("login.xlsx");
		int numOfRows=sheet.getLastRowNum();
		String mail,password,state;
		String results="";
		for(int row=0;row<=numOfRows;row++)
		{
			Thread.sleep(5000);
			String url=firefox.getCurrentUrl();
			firefox.findElement(By.id("itext-mail-phone")).clear();
			firefox.findElement(By.id("ipasswd")).clear();
			
			mail=sheet.getRow(row).getCell(0).getStringCellValue();
			password=sheet.getRow(row).getCell(1).getStringCellValue();
			state=sheet.getRow(row).getCell(2).getStringCellValue();
			
			firefox.findElement(By.id("itext-mail-phone")).sendKeys(mail);
			firefox.findElement(By.id("ipasswd")).sendKeys(password);
			firefox.findElement(By.id("ibtn-login")).click();
			Thread.sleep(10000);
			String currentUrl=firefox.getCurrentUrl();
			
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
		int numOfRows=sheet.getLastRowNum()+1;
		String result="";
		Thread.sleep(3000);
		firefox.findElement(By.id("ibtn-sign-up")).click();
		Thread.sleep(3000);
		String url=firefox.getCurrentUrl();
		for(int row=0;row<numOfRows;row++)
		{
			String fName,mName,lName,nationalIDNum,email,phone,gender,nationalPhoto,
				   password,cPassword,bloodType,date,speciality,professionLicense,state;
			Thread.sleep(10000);
			
			firefox.findElement(By.id("itext-first-name")).clear();
			firefox.findElement(By.id("itext-middle-name")).clear();
			firefox.findElement(By.id("itext-last-name")).clear();
			firefox.findElement(By.id("itext-national-id")).clear();
			firefox.findElement(By.id("ifile-national-id")).clear();
			firefox.findElement(By.id("itext-mail")).clear();
			firefox.findElement(By.id("itext-phone")).clear();
			firefox.findElement(By.id("itext-passwd")).clear();
			firefox.findElement(By.id("itext-passwd-confirm")).clear();
			firefox.findElement(By.id("iselect-blood-type")).sendKeys("Blood Type");
			firefox.findElement(By.id("iselect-day")).sendKeys("Day");
			firefox.findElement(By.id("iselect-month")).sendKeys("Month");
			firefox.findElement(By.id("iselect-year")).sendKeys("Year");
			firefox.findElement(By.id("iselect-speciality")).sendKeys("Speciality");
			firefox.findElement(By.id("ifile-license")).clear();
			
			fName=sheet.getRow(row).getCell(0).getStringCellValue();
			mName=sheet.getRow(row).getCell(1).getStringCellValue();
			lName=sheet.getRow(row).getCell(2).getStringCellValue();
			nationalIDNum=sheet.getRow(row).getCell(3).getStringCellValue();
			email=sheet.getRow(row).getCell(4).getStringCellValue();
			phone=sheet.getRow(row).getCell(5).getStringCellValue();
			gender=sheet.getRow(row).getCell(6).getStringCellValue();
			nationalPhoto=sheet.getRow(row).getCell(7).getStringCellValue();
			password=sheet.getRow(row).getCell(8).getStringCellValue();
			cPassword=sheet.getRow(row).getCell(9).getStringCellValue();
			bloodType=sheet.getRow(row).getCell(10).getStringCellValue();
			date=sheet.getRow(row).getCell(11).getStringCellValue();
			speciality=sheet.getRow(row).getCell(12).getStringCellValue();
			professionLicense=sheet.getRow(row).getCell(13).getStringCellValue();
			
			firefox.findElement(By.id("itext-first-name")).sendKeys(fName);
			firefox.findElement(By.id("itext-middle-name")).sendKeys(mName);
			firefox.findElement(By.id("itext-last-name")).sendKeys(lName);
			firefox.findElement(By.id("itext-national-id")).sendKeys(nationalIDNum);
			firefox.findElement(By.id("itext-mail")).sendKeys(email);
			firefox.findElement(By.id("itext-phone")).sendKeys(phone);
			if(gender.equals("female"))
				firefox.findElement(By.id("iradio-gender-female")).click();
			else if(gender.equals("male"))
				firefox.findElement(By.id("iradio-gender-male")).click();
			if(!nationalPhoto.equals(""))
			{
				firefox.findElement(By.id("ifile-national-id")).sendKeys(nationalPhoto);
			}
			firefox.findElement(By.id("itext-passwd")).sendKeys(password);
			firefox.findElement(By.id("itext-passwd-confirm")).sendKeys(cPassword);
			if(!bloodType.equals(""))
				firefox.findElement(By.id("iselect-blood-type")).sendKeys(bloodType);
			
			String[]dateItems=date.split("-");
			if(dateItems.length==3)
			{
					firefox.findElement(By.id("iselect-day")).sendKeys(dateItems[0]);
					firefox.findElement(By.id("iselect-month")).sendKeys(dateItems[1]);
					firefox.findElement(By.id("iselect-year")).sendKeys(dateItems[2]);
			}
			else if(dateItems.length==2)
			{
				firefox.findElement(By.id("iselect-day")).sendKeys(dateItems[0]);
				firefox.findElement(By.id("iselect-month")).sendKeys(dateItems[1]);
			}
			else if(dateItems.length==1)
			{
				firefox.findElement(By.id("iselect-day")).sendKeys(dateItems[0]);
			}
			firefox.findElement(By.id("iselect-speciality")).sendKeys(speciality);
			if(!professionLicense.equals(""))
				firefox.findElement(By.id("ifile-license")).sendKeys(professionLicense);
			firefox.findElement(By.id("ibtn-sign-up")).click();	
			Thread.sleep(4000);
			String currentUrl=firefox.getCurrentUrl();
			state=sheet.getRow(row).getCell(14).getStringCellValue();
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
				Thread.sleep(4000);
				try {
					firefox.findElement(By.id("ibtn-sign-up")).click();
				}
				catch (NoSuchElementException e){}
			}
			Thread.sleep(20000);
			
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
