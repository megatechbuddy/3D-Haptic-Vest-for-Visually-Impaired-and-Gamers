CON               
  _clkmode = xtal1 + pll16x                             ' Crystal and PLL settings.
  _xinfreq = 5_000_000                                  ' 5 MHz crystal (5 MHz x 16 = 80 MHz).        
  clock = 2
  dataport = 0
  latch = 1
  
  clock2 = 5
  dataport2 = 3
  latch2 = 4
  
OBJ
       
  pst    : "Parallax Serial Terminal"                   ' Serial communication object

VAR
                             
    word Data[24]
    byte outdata[12]
    word Data2[24]
    byte outdata2[12]
    word value                 
    long stack[30]
    byte help 

PUB Main | val, xn, a, b, c, d     
  dira[0..23]~~                    
  dira[6]~
  help := 0 
  cognew(Blink, @stack[0])   
  pst.Start(115200)    'start terminal                                                        
  Data[0] := 0                                         
  Data[1] := 0                                          
  Data[2] := 0                                           
  Data[3] := 0                                          
  Data[4] := 0                                           
  Data[5] := 0                                         
  Data[6] := 0                                                 
  Data[7] := 0                                           
  Data[8] := 0                                           
  Data[9] := 0                                           
  Data[10] := 0                                           
  Data[11] := 0                                          
  Data[12] := 0                                   
  Data[13] := 0                                                
  Data[14] := 0                                           
  Data[15] := 0                                           
  Data[16] := 0                                           
  Data[17] := 0                                           
  Data[18] := 0                                          
  Data[19] := 0                                          
  Data[20] := 0                                            
  Data[21] := 0                                           
  Data[22] := 0                                            
  Data[23] := 0
                                                
  Data2[0] := 0                                         
  Data2[1] := 0                                          
  Data2[2] := 0                                           
  Data2[3] := 0                                          
  Data2[4] := 0                                           
  Data2[5] := 0                                         
  Data2[6] := 0                                                 
  Data2[7] := 0                                           
  Data2[8] := 0                                           
  Data2[9] := 0                                           
  Data2[10] := 0                                           
  Data2[11] := 0                                          
  Data2[12] := 0                                   
  Data2[13] := 0                                                
  Data2[14] := 0                                           
  Data2[15] := 0                                           
  Data2[16] := 0                                           
  Data2[17] := 0                                           
  Data2[18] := 0                                          
  Data2[19] := 0                                          
  Data2[20] := 0                                            
  Data2[21] := 0                                           
  Data2[22] := 0                                            
  Data2[23] := 0
                                                        
  repeat         
    outa[16] := 0
    outa[17] := 0            
    
    'Output Data                    
    b := 0                    
    value := %00000000_00000001
    outa[latch] := 0             
    repeat 24  
      repeat 12   
        outa[clock] := 0                   
        outdata[a] := Data[23-b] & value   
        Data[23-b] := Data[23-b] >> value          
        a += 1
      a := 11                    
      repeat 12  
        outa[dataport] := outdata[a] 
        outa[clock] := 1  
        outa[clock] := 0  
        a -= 1
                          
      b += 1
      a := 0     
    outa[latch] := 1
    outa[latch] := 0
     
     
    'Output Data       
    value := %00000000_00000001          
    b := 0
    outa[latch2] := 0
    repeat 24  
      repeat 12   
        outa[clock2] := 0                   
        outdata2[a] := Data2[23-b] & value   
        Data2[23-b] := Data2[23-b] >> value          
        a += 1
      a := 11                    
      repeat 12  
        outa[dataport2] := outdata2[a] 
        outa[clock2] := 1  
        outa[clock2] := 0  
        a -= 1
                          
      b += 1
      a := 0     
    outa[latch2] := 1
    outa[latch2] := 0

                       
    outa[16] := 1      
                                                 
    c := 0   
    d:=0
    repeat until c == 1
      d := pst.DecIn
      if d > 0                                    
        c := 1                   
        outa[17] := 1      
    if d/1000 == 9             
      outa[18] := 1     
      b := 0     
      repeat 12 
        Data[b] := 4095  'recieve data   
        b := b + 1            
    elseif d/1000 < 9             
      outa[18] := 0            
      b := 0     
      repeat 12 
        Data[b] := 0  'recieve data   
        b := b + 1            
    d:=d-((d/1000)*1000)
      
    if d/100 == 9             
      outa[19] := 1              
      b := 12     
      repeat 12 
        Data[b] := 4095  'recieve data   
        b := b + 1       
    elseif d/100 < 9             
      outa[19] := 0             
      b := 12     
      repeat 12 
        Data[b] := 0  'recieve data   
        b := b + 1     
    d:=d-((d/100)*100)   
      
    if d/10 == 9             
      outa[20] := 1          
      b := 0     
      repeat 12 
        Data2[b] := 4095  'recieve data   
        b := b + 1       
    elseif d/10 < 9             
      outa[20] := 0         
      b := 0     
      repeat 12 
        Data2[b] := 0  'recieve data   
        b := b + 1           
    d:=d-((d/10)*10)
       
    if d == 9             
      outa[21] := 1        
      b := 12     
      repeat 12 
        Data2[b] := 4095  'recieve data   
        b := b + 1       
    elseif d < 9             
      outa[21] := 0        
      b := 12     
      repeat 12 
        Data2[b] := 0  'recieve data   
        b := b + 1       
      
                      
    c := 0
    b := 0                
                                       
                        
PUB Blink | a             
  dira[14..23]~~             
  dira[6]~
  a := 0
  repeat               
    repeat until a == 1
      a := INA[6]        
      outa[23] := INA[6]  
    waitcnt(clkfreq/100000 + cnt)                   ' Delay for 1/20th s      
    a := 0          
    help := 1    
  
DAT                       
   Str0   byte   "a", 0   