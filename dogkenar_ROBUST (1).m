clear all;clc;format long g;
warning('off');
if exist('Dengkx1.xls', 'file') delete('Dengkx1.xls'); end
if exist('Dengky1.xls', 'file') delete('Dengky1.xls'); end

DOSYA='caspary_2.xlsx';
fid1=fopen('C:\Users\Berkant\Desktop\GALERÝLER ÝLE KRET BÝRLÝKTE YERSEL\DENGELEME AÞAMASI\caspary_2.txt','wt'); % Serbest dengeleme sonuçalrinin yazdirildigimatris.
fprintf(fid1,'+ÝTERASYONLU SERBEST DENGELEME SONUÇ RAPORU (DOÐRULTU-KENAR AÐI)+\n');
fprintf(fid1,'+Research Asistant Berkant KONAKOÐLU+\n');
fprintf(fid1,'*************************************\n');

%%DOÐRULTU-KENAR VE KOORDÝNAT OKUTULMASI
DOG=xlsread(DOSYA,'doðrultu');
KEN=xlsread(DOSYA,'kenar');
KOORD=xlsread(DOSYA,'koord');
NN=KOORD(:,1);
Y1=KOORD(:,2);
X1=KOORD(:,3);[u,~]=size(NN);
%toplam
top_y1=sum(Y1);top_x1=sum(X1);
%ortalama
mean_y1=mean(Y1);mean_x1=mean(X1);

%1.per koor-ort X

for i=1:size(NN,1)
    koor_ort1X(i,1)=X1(i,1)-mean_x1;
end

%1.per koor-ort Y

for i=1:size(NN,1)
    koor_ort1Y(i,1)=Y1(i,1)-mean_y1;
end


%% BÝLÝNMEYEN NOKTALARIN YAKLAÞIK KOORDÝNATLARININ YAZDIRILMASI
fprintf(fid1,'++++++++++++++++++++++++++++++++++++++++++++++++++ \n');
fprintf(fid1,'++ BÝLÝNMEYEN NOKTALARIN YAKLAÞIK KOORDÝNATLARI ++ \n');
fprintf(fid1,'++++++++++++++++++++++++++++++++++++++++++++++++++ \n');
fprintf(fid1,'  NNO              Y(m)               X(m)         \n');
for i=1:u;
    fprintf(fid1,'%5u   %18.5f  %18.5f     \n\', NN(i), Y1(i), X1(i));
end
fprintf(fid1,'-------------------------------------------------- \n\n');

%%

DN=DOG(:,1);
BN=DOG(:,2);
DOD=DOG(:,3);
Pvek_d=DOG(:,8);
[nd,m]=size(DOG);
S0=1;
DNK=KEN(:,1);
BNK=KEN(:,2);
KENAR=KEN(:,3);
Pvek_k=KEN(:,8);
[nk,m]=size(KEN);

%% YÖNELTME BÝLÝNMEYENÝ SAYISININ HESABI
aa=1;
sayac=1;
for i=1:nd-1;
    if DN(i+1)==DN(i);
        sayac=sayac+1;
        aa=aa;
        BNS(aa)=sayac;
    else
        sayac=1;
        aa=aa+1;
        BNS(aa)=sayac;
    end
end

YBS=aa; %YÖNELTME BÝLÝNMEYENÝ SAYISI
BS=2*u+YBS; %BÝLÝNMEYEN SAYISI
%% DOÐRULTU ÝÇÝN SEMT VE MESAFELERÝN HESAPLANMASI
for i=1:u;
    kx1(NN(i))=X1(i);
    ky1(NN(i))=Y1(i);
end

for i=1:nd
    dx(i)=kx1(BN(i))-kx1(DN(i));
    dy(i)=ky1(BN(i))-ky1(DN(i));
    Kenar(i)=sqrt(dx(i)^2+dy(i)^2);
    alfa(i)=atan(dy(i)/dx(i));
    alfa(i)=alfa(i)*200/pi;
    
    if dy(i)>0 & dx(i)>0
        Semt(i)=alfa(i);
        elseif dy(i)>0 & dx(i)<0
        Semt(i)=alfa(i)+200;
        elseif dy(i)<0 & dx(i)<0
        Semt(i)=alfa(i)+200;
        elseif dy(i)<0 & dx(i)>0
        Semt(i)=alfa(i)+400;
        end
end
Semt=Semt';
Kenar=Kenar';
%% YÖNELTME BÝLÝNMEYENÝ HESABI
for i=1:nd
    zy(i)=Semt(i)-DOD(i);
end
sayac2=0;
for aa=1:YBS
    sayac1=0;
    k1=BNS(aa);
    k=k1+sayac2;
    for i=sayac2+1:k
        z(i)=Semt(i)'-DOD(i);
        
        if z(i)<0
            z(i)=z(i)+400;
        end
        sayac1=sayac1+z(i);
        sayac2=sayac2+1;
    end
    z0(aa)=sayac1/BNS(aa);
end

sayac2=0;
for aa=1:YBS
    k1=BNS(aa);
    k=k1+sayac2;
    for i=sayac2+1:k
        sabit(i)=z(i)-z0(aa);
        sabit(i)=-sabit(i)*10000;
        sayac2=sayac2+1;
    end
end
ro=200/pi;
ros=ro*10000;
%% DOÐRULTU ÝÇÝN AÝK ve BÝK KATSAYILARIN HESAPLANMASI
for p=1:1:nd;
    Semt(p)=Semt(p)/ro;
    Aik(p)=-sin(Semt(p))*ros/(Kenar(p)*100); % cc/cm
    Bik(p)=cos(Semt(p))*ros/(Kenar(p)*100); % cc/cm
end
%% DOÐRULTU KATSAYILAR MATRÝSÝ
Katsayi=zeros(nd,u*2);
for i=1:nd
    i1=find(ismember(NN,DN(i)));
    i2=find(ismember(NN,BN(i)));
    Katsayi(i,2*i1-1)=-Aik(i);
    Katsayi(i,2*i1)=-Bik(i);
    Katsayi(i,2*i2-1)=Aik(i);
    Katsayi(i,2*i2)=Bik(i);
end
%% DOÐRULTU KATSAYILAR MATRÝSÝ ÝÇÝN YÖNELTME BÝLÝNMEYENÝ DENKLEMÝ

il=1;
for k=1:YBS
    nz=BNS(k);
    jl=il;
    for j=1:2*u
        s=0;
        
        for i=1:nz
            s=s+Katsayi(jl,j);
            takat(k,j)=s/nz;
            jl=jl+1;
        end
        
        jl=il;
        
        for i=1:nz;
            AIK(jl,j)=Katsayi(jl,j)-takat(k,j);
            jl=jl+1;
        end
        jl=il;
    end
    il=il+nz;
end
%% DOÐRULTU L VEKTÖRÜ ÝÇÝN YÖNELTME BÝLÝNMEYENÝ DENKLEMÝ

il=1;
for k=1:YBS
    nz=BNS(k);
    jl=il;
        s=0;
        
        for i=1:nz
            s=s+sabit(jl);
            tsabit(k)=s/nz;
            jl=jl+1;
        end
        
        jl=il;
        
        for i=1:nz
            sabiti(jl)=sabit(jl)-tsabit(k);
            jl=jl+1;
        end
        
        il=il+nz;
end

sabiti=sabiti';
% %% FARKLARIN HESAPLANMASI (l DEÐERLERÝ)--DOÐRULTU--
% for p=1:1:nd;
% lid(p)=Semt(p)-DOD(p);
% if lid(p)<0;
% lid(p)=lid(p)+400;
% end
% end
% lid=lid';

%% KENAR ÝÇÝN AÝKK ve BÝKK KATSAYILARIN HESAPLANMASI
clear Semt Kenar 
for i=1:nk
    dxk(i)=kx1(BNK(i))-kx1(DNK(i));
    dyk(i)=ky1(BNK(i))-ky1(DNK(i));
    Kenar(i)=sqrt(dxk(i)^2+dyk(i)^2);
    alfa(i)=atan(dyk(i)/dxk(i));
    alfa(i)=alfa(i)*200/pi;
    
    if dyk(i)>0 & dxk(i)>0
        Semt(i)=alfa(i);
        elseif dyk(i)>0 & dxk(i)<0
        Semt(i)=alfa(i)+200;
        elseif dyk(i)<0 & dxk(i)<0
        Semt(i)=alfa(i)+200;
        elseif dyk(i)<0 & dxk(i)>0
        Semt(i)=alfa(i)+400;
        end



Aikk(i)=dxk(i)/Kenar(i)';
Bikk(i)=dyk(i)/Kenar(i)';
end
Semt=Semt';
Kenar=Kenar';

%% FARKLARIN HESAPLANMASI (l DEÐERLERÝ)--KENAR--
for p=1:nk;
lk1(p)=(KENAR(p)-Kenar(p))*100;
end

lk1=lk1';
fprintf(fid1,'++++++++++++++++ \n');
fprintf(fid1,'++ Verilenler ++ \n');
fprintf(fid1,'++++++++++++++++ \n\n');

fprintf(fid1,'--------------------------------------------------------------------------- \n');
fprintf(fid1,'  DN         BN      Ölç. Kenar(m)    Hes. Kenar(m)      lvek(cm)    \n');
fprintf(fid1,'--------------------------------------------------------------------------- \n');
for i=1:nk;
    fprintf(fid1,'%5u %10u %15.4f %15.4f %12.2f \n', DNK(i),BNK(i),KENAR(i),Kenar(i),lk1(i));
end



%% KENARLAR ÝÇÝN KATSAYILAR MATRÝSÝ
KatsayiK=zeros(nk,u*2);
for i=1:nk
    i1=find(ismember(NN,DNK(i)));
    i2=find(ismember(NN,BNK(i)));
    KatsayiK(i,2*i1-1)=-Aikk(i);
    KatsayiK(i,2*i1)=-Bikk(i);
    KatsayiK(i,2*i2-1)=Aikk(i);
    KatsayiK(i,2*i2)=Bikk(i);
end


%% DOÐRULTU VE KENAR MATRÝSLERÝNÝ BÝRLEÞTÝRME
ndk=nd+nk;
d=3;
AMAT=[AIK;KatsayiK];
LMAT=[sabiti;lk1];
AGIRLIK=[Pvek_d;Pvek_k];
AGIRLIK=diag(AGIRLIK);

%% DENGELEME

xbilinmeyen_old=pinv(AMAT'*AGIRLIK*AMAT)*(AMAT'*AGIRLIK*LMAT);
%BÝLÝNMEYENLER VEKTÖRÜ
V=(AMAT*xbilinmeyen_old)-LMAT;%DÜZELTMELERÝN HESABI
f=ndk-BS+d;%SERBEST DERECESÝ u=2 alýndý
M0=sqrt((V'*AGIRLIK*V)/(f));%BÝRÝM ÖLÇÜNÜN KARASEL ORTALAMA HATASI

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
alfa1=0.95;
tdist=tinv(alfa1,f);
QVV=pinv(AGIRLIK)-(AMAT*pinv(AMAT'*AGIRLIK*AMAT)*AMAT');
c_sinir=(M0.*diag(sqrt(QVV)).*diag(sqrt(AGIRLIK))*tdist);
c_sinir=sum(c_sinir)/(nd+nk);
iterasyon=0;
for it=1:1000
    iterasyon=iterasyon+1;
fprintf(fid1,'+-+-+-+-+-+-+-+-+-+-+-+-+-+-+ \n');  
fprintf(fid1,'+ Ýterasyon Sayýsý =\t %i  +\n',it);
fprintf(fid1,'+-+-+-+-+-+-+-+-+-+-+-+-+-+-+ \n\n');  

%% DOÐRULTU VE KENAR MATRÝSLERÝNÝ BÝRLEÞTÝRME
ndk=nd+nk;
d=3;
AMAT=[AIK;KatsayiK];
LMAT=[sabiti;lk1];

fprintf(fid1,'++++++++++++++++++++++++++++++++++ \n');
fprintf(fid1,'AÐA ÝLÝÞKÝN BÝLGÝLER             +\n');
fprintf(fid1,'Doðrultu Ölçü Sayýsý    nd = %3i +\n',nd);
fprintf(fid1,'Yöneltme Bilinmeyeni Sayisi= %3i + \n\', YBS);
fprintf(fid1,'Kenar Ölçü Sayýsý       nk = %3i +\n',nk);
fprintf(fid1,'Bilinmeyen Nokta Sayýsý  u = %3i +\n',u);
fprintf(fid1,'Datum Defekti            d = %3i +\n',d);
fprintf(fid1,'Serbestlik Derecesi  n-u+d = %3i +\n',ndk-u*2-YBS+d);
fprintf(fid1,'++++++++++++++++++++++++++++++++++ \n\n');


%% DENGELEME

xbilinmeyen_oldD=pinv(AMAT'*AGIRLIK*AMAT)*(AMAT'*AGIRLIK*LMAT);
%BÝLÝNMEYENLER VEKTÖRÜ
V=(AMAT*xbilinmeyen_oldD)-LMAT;%DÜZELTMELERÝN HESABI
f=ndk-BS+d;%SERBEST DERECESÝ u=2 alýndý
M0=sqrt((V'*AGIRLIK*V)/(f));%BÝRÝM ÖLÇÜNÜN KARASEL ORTALAMA HATASI
%% andrew UYUÞUMSUZ ÖLÇÜLERÝN TEST BÜYÜKLÜKLERÝ
% %güven aralýðý deðeri

c=1.5;
zz=find(abs(V)>c_sinir*pi);

W=diag((((abs(V)/c_sinir).^(-1))).*sin(abs(V)/c_sinir));
W(zz,zz)=0;

AGIRLIK_1=W;
AGIRLIK=AGIRLIK*AGIRLIK_1;

xbilinmeyen_new=pinv(AMAT'*AGIRLIK*AMAT)*(AMAT'*AGIRLIK*LMAT);
abs(xbilinmeyen_new-xbilinmeyen_oldD)

if abs(xbilinmeyen_new-xbilinmeyen_oldD)<=0.001;


    break
end
end


%% DENETÝM
V_son=(AMAT*xbilinmeyen_new)-LMAT;%DÜZELTMELERÝN HESABI
VtPV=V_son'*AGIRLIK*V_son;
VtPL=-V_son'*AGIRLIK*LMAT;
LPLt_xtAPLt=(LMAT'*AGIRLIK*LMAT)-xbilinmeyen_new'*AMAT'*AGIRLIK*LMAT;
fprintf(fid1,'++++++++++++++++++++++++++++++++ \n');
fprintf(fid1,'++     SONUÇ DENETÝMLERÝ      ++    \n');
fprintf(fid1,'++     -----------------      ++\n');
fprintf(fid1,'++ VtPV        = %5.5f    ++\n\', VtPV );
fprintf(fid1,'++ VtPL        = %5.5f    ++\n\', VtPL);
fprintf(fid1,'++ LPLt_xtAPLt = %5.5f    ++\n\', LPLt_xtAPLt);
fprintf(fid1,'++++++++++++++++++++++++++++++++ \n\n');
%%
%BÝLÝNMEYENLERÝN TERS AÐIRLIK MATRÝSÝ
N=AMAT'*AGIRLIK*AMAT;
Qxx=pinv(N);
xlswrite('Qxx.xlsx',Qxx);
xlswrite('N.xlsx',N);
% for i=1:u*2
%     Qxx_bagil(i,i)=sqrt(Qxx(i,i)).*M0;
% end
% 
% %baðýl deformasyon çýktýlarý
% xlswrite('Qxx_baðýl.xlsx',Qxx_bagil);
%% DENGELÝ ÖLÇÜLERÝN VARYANS-KOVARYANS MATRÝSÝ Kxx=mo^2*Qxx;
Kxx=M0^2*Qxx;
% xlswrite('Kxx.xlsx',Kxx);
%% DENGELÝ ÖLÇÜLERÝN TERS AÐIRLIKLARI Qll=(akati*Qxx*akati');
Qll=AMAT*Qxx*AMAT';
%% DENGELÝ ÖLÇÜLERÝN VARYANS-KOVARYANS MATRÝSÝ Kll=mo^2*Qll;

Kll=M0^2*Qll;

%% DÜZELTMELERÝN TERS AÐIRLIK MATRÝSÝ

Qvv=pinv(AGIRLIK)-Qll;
%%REDUNDANZ PAYI
Rdnz=diag(Qvv*AGIRLIK);

for i=1:nd Rdnz_dog(i)=Rdnz(i); end
for j=1:nk 
    Rdnz_ken(j)=Rdnz(j+nd); 
end

Rdnz_dog=(Rdnz_dog)';
Rdnz_ken=(Rdnz_ken)';

fprintf(fid1,'DN       BN         Redundanzlar          Sýnýr Deðeri \n');
fprintf(fid1,'                        r_dog              0.3  veya  0.5 \n');
fprintf(fid1,'----------------------------------------------------------- \n');
for i=1:length(DN)
    Redundanz_dog(3*i-2,1)=DN(i);
    Redundanz_dog(3*i-1,1)=BN(i);
    Redundanz_dog(3*i,1)=Rdnz_dog(i);
end
fprintf(fid1,'%0.f %8.f %10.4f \n',Redundanz_dog);

fprintf(fid1,'DN       BN         Redundanzlar          Sýnýr Deðeri \n');
fprintf(fid1,'                        r_ken              0.3  or  0.5 \n');
fprintf(fid1,'----------------------------------------------------------- \n');

for i=1:length(DNK)
    Redundanz_ken(3*i-2,1)=DNK(i);
    Redundanz_ken(3*i-1,1)=BNK(i);
    Redundanz_ken(3*i,1)=Rdnz_ken(i);
end

fprintf(fid1,'%0.f %8.f %10.4f \n',Redundanz_ken);


%% x ve y koordinatlarýn karesel ortalama hatasý

for i=1:u
mx(i)=(sqrt(Kxx((i*2-1),(i*2-1))));    
my(i)=(sqrt(Kxx((i*2),(i*2))));        
end

%% NOKTALARIN ORTALAMA KOORDÝNAT DUYARLIÐI

mxy=M0*sqrt(trace(pinv(AMAT'*AGIRLIK*AMAT))/2*u); 

%% ORTALAMA KONUM DUYARLIÐI

for i=1:u
mp(i)=sqrt(mx(i)^2+my(i)^2);              
end

%% DÜZELTMELERÝN SEMT HESABI
[nnn,uuu]=size(AMAT);

for i=1:nd
    dendog(i)=DOD(i)+V(i)./10000;
end

for p=1:uuu/2;
    YXMAT(p,1)=xbilinmeyen_new(2*p,1);
end

for p=1:uuu/2;
    XXMAT(p,1)=xbilinmeyen_new(2*p-1,1);
end

Dengky1=KOORD(:,2)+YXMAT./100;
Dengkx1=KOORD(:,3)+XXMAT./100;

%% DOÐRULTU DÜZELTMELERÝN YAZDIRILMASI
fprintf(fid1,'++++++++++++++++++++++++++++++++++++++ \n');
fprintf(fid1,'++ DÜZELTME DEÐERLERÝ(V) (DOÐRULTU) ++                                                                            \n');
fprintf(fid1,'++++++++++++++++++++++++++++++++++++++ \n');
fprintf(fid1,'DN         BN        Düzeltme(cc)        \n');
fprintf(fid1,'--------------------------------- \n');
for i=1:nd;
    fprintf(fid1,'%3u %10u %15.5f  \n\', DN(i),BN(i),V(i));
end
%% KENAR DÜZELTMELERÝN YAZDIRILMASI
fprintf(fid1,'++++++++++++++++++++++++++++++++++++++ \n');
fprintf(fid1,'++ DÜZELTME DEÐERLERÝ(V) (KENAR)    ++                                                                            \n');
fprintf(fid1,'++++++++++++++++++++++++++++++++++++++ \n');
fprintf(fid1,'DN         BN        Düzeltme(cc)        \n');
fprintf(fid1,'--------------------------------- \n');
for i=1:nk;
    fprintf(fid1,'%3u %10u %15.5f  \n\', DNK(i),BNK(i),V(i+nd));
end
%% DENGELÝ NOKTA KOORDÝNATLARIN YAZDIRILMASI
fprintf(fid1,'++++++++++++++++++++++++++++++++++++++ \n');
fprintf(fid1,'++   DENGELÝ NOKTA KOORDÝNATLARI    ++ \n');
fprintf(fid1,'++++++++++++++++++++++++++++++++++++++ \n');
fprintf(fid1,'NNO            Y(m)            X(m)    \n');
fprintf(fid1,'-------------------------------------- \n');
for i=1:u;
    fprintf(fid1,'%0u   %15.5f  %15.5f  \n\', NN(i), Dengky1(i), Dengkx1(i));
end
%% BÝRÝM ÖLÇÜNÜN KARESEL ORTALAMA HATASI
fprintf(fid1,'+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+ \n');  
fprintf(fid1,'++ Birim Ölçünün Karesel Ortalama Hatasi(cc) = %5.5f ++ \n\', M0);
fprintf(fid1,'+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+ \n\n');  

%% KOORDÝNATLARA AÝT KAR. ORT. HATALAR VE NOKTA KONUM DUYARLIKLARIN YAZDIRILMASI
fprintf(fid1,'Koordinatlara Ait Karesel Ortalama Hatalar ve Nokta Konum Duyarlýklarý:      \n\' );
fprintf(fid1,'+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+ \n');
fprintf(fid1,'NNO     mx     my    mp(cm)                                      \n');
for i=1:u;
    fprintf(fid1,'%0u   %5.4f  %5.4f %5.4f  \n\', NN(i), my(i), mx(i), mp(i));
end
%% KOORDÝNATLARIN EXCELE AKTARILMASI 
xlswrite('Dengkx1',Dengkx1);xlswrite('Dengky1',Dengky1);

% xlswrite('Kll_I7_kret',Kll);

%% DOÐRULTU KENAR AÐININ ÇÝZDÝRÝLMESÝ

figure('name','DOÐRULTU-KENAR AÐI','numbertitle','off');
grid on;
title('DOÐRULTU-KENAR AÐI','Color','black','Fontweight','bold');


for i=1:u
    
    text(X1(i)+10,Y1(i)+10,['N.',num2str(NN(i))],'FontWeight','bold')  

end
hold on

for i=1:length(DN)
   Durulan=find(ismember(NN,DN(i)));
   Bakilan=find(ismember(NN,BN(i)));
   
   plot([X1(Durulan),X1(Bakilan)],[Y1(Durulan),Y1(Bakilan)],...
       'Color','black','Marker','^','LineWidth',1);
end

set(gca, 'XTickLabel', num2str(get(gca,'XTick')','%d'));
set(gca, 'YTickLabel', num2str(get(gca,'YTick')','%d'));
