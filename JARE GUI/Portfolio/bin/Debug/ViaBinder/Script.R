numberOfObjectives = 2
#Naziv fajla sa weights kao argument skript fajla
args=commandArgs(trailingOnly = TRUE)
weightsFileName=args[1]
weights= read.table(weightsFileName, header=FALSE)
w=data.matrix(weights,rownames.force=NA)
#Ucitavanje vremenskih serija
d=read.table("timeSeries.csv-uploaded", header=FALSE, sep=',')
m=data.matrix(d,rownames.force=NA)[,-1]
#Racunanje vrednosti portfolija
#Vrednost holdingsa se racuna na osnovu capital invested weights i vrednosti aseta na poslednji dan evaluacije
timeSeriesRowCount = nrow(m)
assetCount = ncol(m)
shares=matrix(0,nrow=assetCount,ncol=1)
portfolioValue=1
for(i in 1:assetCount) shares[i]=w[i]*portfolioValue/m[timeSeriesRowCount,i]
p=m%*%shares
#Racunanje prinosa portfolija
yield=matrix(,ncol=1)
for( i in 1:1000) {yield[i]=p[i+1]/p[i]-1}
r=matrix(yield,ncol=1)
averageReturn=-mean(r)
#Lodovanje biblioteke rugacrch
library(rugarch)
library(simsalapar)
#Definisanje GARCH modela
sp<-ugarchspec (variance.model = list(model = 'sGARCH', garchOrder = c(1, 1), submodel = NULL, external.regressors = NULL, variance.targeting = FALSE), mean.model = list(armaOrder = c(1, 1), include.mean = FALSE, archm = FALSE, archpow = 1, arfima = FALSE, external.regressors = NULL, archex = FALSE), distribution.model = 'std', start.pars = list(), fixed.pars = list())
#Fitovanje GARCH modela
DONE=TRUE
MESSAGE="OK"
tryObject=tryCatch.W.E(ugarchfit(data = r, spec = sp))
if(inherits(tryObject$value, "error") || is.null(coef(tryObject$value)['ar1'])){
DONE=FALSE
if(!is.null(tryObject$warning)){
MESSAGE=tryObject$warning$message
}
else if(!is.null(tryObject$error)){
MESSAGE=tryObject$error$message
}
}else{
fit = tryObject$value
df=coef(fit)['shape']
#Predikcija VaRa
tryObject2=tryCatch.W.E(ugarchforecast(fit, n.ahead=1))
if(inherits(tryObject2$value, "error")){
DONE=FALSE
MESSAGE=tryObject2$value$message
}else{
forc = tryObject2$value
sigma=sigma(forc)
mean=fitted(forc)
quantil= qdist('std', p= 0.01, mu = 0, sigma = 1, shape=df)
#Odredjivanje VaR-a
VaR = -sigma*quantil
}
}
if(!DONE)
{
VaR = 1000
averageReturn = 1000
}
{
cat("OK
")
cat(numberOfObjectives, "
")
cat("averageReturn=", averageReturn, "
")
cat("VaR=", VaR, "
")
}
