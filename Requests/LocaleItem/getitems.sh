#!/bin/bash

curl -X GET -H "Content-Type: application/json" "http://localhost:5028/localeitem/match?lang=it&context=app&content=ciao%20amore" | jq .
curl -X GET -H "Content-Type: application/json" "http://localhost:5028/localeitem/search?lang=it&context=app&content=que" | jq .
curl -X GET -H "Content-Type: application/json" "http://localhost:5028/localeitem/search?lang=it&content=que" | jq .
# curl -X GET "http://localhost:5028/localeitem/searchx?lang=it&context=app&content=ciao"
# curl -X GET http://localhost:5028/localeitem/searchx?context=app&lang=it&content=ciao | jq .
# curl -X GET http://localhost:5028/localeitem/searchx?content=ciao&lang=it&context=app | jq .
# curl -X GET http://localhost:5028/localeitem/search?lang=it&context=app&content=ciao | jq .
# curl -X GET http://localhost:5028/localeitem/search?context=app&lang=it&content=ciao | jq .
# curl -X GET http://localhost:5028/localeitem/search?content=ciao&lang=it&context=app | jq .
