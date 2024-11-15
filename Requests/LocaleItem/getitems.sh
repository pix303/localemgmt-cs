#!/bin/bash

curl -X GET -H "Content-Type: application/json" "http://localhost:5028/localeitem/match?lang=it&context=app&content=ciao%20amore" | jq .
curl -X GET -H "Content-Type: application/json" "http://localhost:5028/localeitem/match?lang=it&context=app&content=ciao%20amorex" | jq .
curl -X GET -H "Content-Type: application/json" "http://localhost:5028/localeitem/search?lang=it&context=app&content=e" | jq .
curl -X GET -H "Content-Type: application/json" "http://localhost:5028/localeitem/search?lang=it&content=que" | jq .
