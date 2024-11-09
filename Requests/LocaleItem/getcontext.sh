#!/bin/bash

curl -X GET http://localhost:5028/localeitem/context/app | jq .
curl -X GET http://localhost:5028/localeitem/context/app?lang=it | jq .
