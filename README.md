# Descriere

Proiect de template pentru dezvoltarea de aplicatii care sa se integreze cu platforma DNN.
Proiectul este facut sa ruleze ca parte integranta a DNN.

# Setup

1. Se instaleaza DNN urmand pasii din [aceste video-uri](https://www.youtube.com/playlist?list=PLFpEtny5sIbbT0spov4It4Z8fswIbNZFd) 
_IMPORTANT - nu uitati parola pe care o setati la contul de host in timpul instalarii_
2. Se descarca acest proiect intr-un folder din dnndev.me/DesktopModules. Proiectul e configurat sa aiba output path-ul de la build in folderul build din dnn, astfel incat dll-urile rezultate sa fie rulate de acolo.
3. Se elimina legatura cu acest repository `rm -rf .git`
4. Se ruleaza Visual Studio ca administrator
5. Se redenumeste proiectul si toate namespace-urile care deriva din numele proiectului
6. Se inlocuieste in clasa RouteMapper.cs `WebApplication1` cu noul nume al proiectului
7. Pentru a rula proiectul acesta se va compila -> este important ca dll-rile rezultate sa fie generate in fisierul bin din DNN 
8. Endpoint-urile vor fi disponibile cu URL-ul de baza `http://dnndev.me/api/WebApplication1/`, ex `http://dnndev.me/api/WebApplication1/Demo/get`

# Fisierul Web.config

Proiectul ruleaza in cadrul DNN, asa ca in final va folosi fisierul Web.config din interiorul DNN. Astfel connection strings-urile si alte setari vor trebui copiate acolo.

# Debugging

Pentru a se face debugging in Visual Studio pe un proiect care ruleaza pe IIS se merge la meniul Debug -> Attach to process -> se ataseaza de procesul w3wp.exe(daca acesta nu este prezent se face un request la ceva ce tine de DNN si acesta va porni) 

# Securizarea folosind rolurile din DNN

DNN permite crearea de roluri pentru utilizatori, pe baza carora se poate restrictiona accesul la endpoint-uri prin folosirea de atribute specifice, ex `[DnnAuthorize(StaticRoles = "Cadru Didactic")]`
