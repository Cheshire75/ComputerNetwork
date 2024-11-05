import java.awt.Container;
import java.io.File;

import javax.swing.*;

public class UI extends JFrame{
	
	
	public UI()
	{
		setTitle("메일 전송 프로그램");
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setSize(510,420);
		
		Container contentPane = getContentPane();
		contentPane.setLayout(null);
		
		JLabel sendAddrLabel = new JLabel("보내는 주소");
		sendAddrLabel.setBounds(10,10,80,20);
		contentPane.add(sendAddrLabel);
		
		JTextField sendAddrText = new JTextField();
		sendAddrText.setBounds(80,12,400,20);
		contentPane.add(sendAddrText);
		
		JLabel sendPWLabel = new JLabel("비밀번호");
		sendPWLabel.setBounds(10,35,80,20);
		contentPane.add(sendPWLabel);
		
		JTextField sendPWText = new JTextField();
		sendPWText.setBounds(80, 37, 400, 20);
		contentPane.add(sendPWText);
		
		JLabel rcptLabel = new JLabel("받는 주소");
		rcptLabel.setBounds(10,60,80,20);
		contentPane.add(rcptLabel);
		
		JTextField rcptText = new JTextField();
		rcptText.setBounds(80,62,400,20);
		contentPane.add(rcptText);
		
		JLabel subjectLabel = new JLabel("제목");
		subjectLabel.setBounds(10, 95, 80, 20);
		contentPane.add(subjectLabel);
		
		JTextField subjectText = new JTextField();
		subjectText.setBounds(80, 97, 400, 20);
		contentPane.add(subjectText);
		
		JLabel bodyLabel = new JLabel("내용");
		bodyLabel.setBounds(10, 120, 80, 20);
		contentPane.add(bodyLabel);

		JTextArea bodyText = new JTextArea();
		bodyText.setLineWrap(true);
		bodyText.setWrapStyleWord(true);
		
		JScrollPane scrollPane = new JScrollPane(bodyText);
		
		scrollPane.setBounds(80, 122, 400, 200);
		contentPane.add(scrollPane);
		
		JButton sendButton = new JButton("메일 전송");
		sendButton.setBounds(380,355,100,20);
		contentPane.add(sendButton);
		
		JButton clearButton = new JButton("양식 비우기");
		clearButton.setBounds(270, 355, 100, 20);
		contentPane.add(clearButton);
		
		JButton selectFile = new JButton("파일 첨부");
		selectFile.setBounds(80, 330, 100, 20);
		contentPane.add(selectFile);
		
		JLabel fileDir = new JLabel("");
		fileDir.setBounds(190, 330, 290, 20);
		contentPane.add(fileDir);
		
		final File[] file = new File[1];
		
		selectFile.addActionListener(e->{
			JFileChooser fileChooser = new JFileChooser();
			int result = fileChooser.showOpenDialog(this);
			
			if(result == JFileChooser.APPROVE_OPTION)
			{
				file[0] = fileChooser.getSelectedFile();
				fileDir.setText(file[0].getAbsolutePath());
			}
		});
		
		setVisible(true);
		
		clearButton.addActionListener(e ->{
			sendAddrText.setText("");
			sendPWText.setText("");
			rcptText.setText("");
			subjectText.setText("");
			bodyText.setText("");
		});
		
		sendButton.addActionListener(e->{
			String sender = sendAddrText.getText();
			String host="";
			if(sender.contains("@"))
			{
				String[] tmp = sender.split("@");
				host="smtp."+tmp[1];
			}
			String password = sendPWText.getText();
			String rcpt=rcptText.getText();
			String subject = subjectText.getText();
			String body = bodyText.getText();
			
			MailClient client = new MailClient(host);
			try {
				client.sendEmail(sender, password, rcpt, subject, body, file[0]);
				JOptionPane.showMessageDialog(null, "메일이 성공적으로 전송되었습니다.","Info",JOptionPane.INFORMATION_MESSAGE);
			}
			catch(Exception error) {
				JOptionPane.showMessageDialog(null, error.getMessage(),"Warning",JOptionPane.WARNING_MESSAGE);
			}
		});
	}
	
	public static void main(String [] args)
	{
		UI client = new UI();
	}
}
