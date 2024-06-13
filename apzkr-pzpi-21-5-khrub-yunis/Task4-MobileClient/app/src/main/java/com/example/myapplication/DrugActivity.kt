package com.example.myapplication.ui

import android.os.Bundle
import android.util.Log
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import com.example.myapplication.R
import com.example.myapplication.model.Drug
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import okhttp3.*
import okhttp3.logging.HttpLoggingInterceptor
import java.io.IOException
import java.security.KeyStore
import javax.net.ssl.*

class DrugActivity : AppCompatActivity() {

    private lateinit var drugListTextView: TextView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_drug)

        drugListTextView = findViewById(R.id.drugListTextView)

        makeHttpRequest()
    }

    private fun makeHttpRequest() {
        val client = getUnsafeOkHttpClient()

        val request = Request.Builder()
            .url("https://10.0.2.2:7126/api/Mobile/drugs-all")
            .build()

        Log.d("DrugActivity", "Making request to URL: ${request.url}")

        client.newCall(request).enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                Log.e("DrugActivity", "HTTP request failed: ${e.message}")
                runOnUiThread {
                    drugListTextView.text = "HTTP request failed: ${e.message}"
                }
            }

            override fun onResponse(call: Call, response: Response) {
                response.use {
                    if (!response.isSuccessful) {
                        Log.e("DrugActivity", "HTTP request failed: ${response.message}")
                        runOnUiThread {
                            drugListTextView.text = "HTTP request failed: ${response.message}"
                        }
                        return
                    }

                    val responseBody = response.body?.string()
                    if (responseBody != null) {
                        val gson = Gson()
                        val listType = object : TypeToken<List<Drug>>() {}.type
                        val drugs: List<Drug> = gson.fromJson(responseBody, listType)

                        val drugsText = drugs.joinToString(separator = "\n") { drug ->
                            """
                            ID: ${drug.drugID}
                            Name: ${drug.drugName}
                            Description: ${drug.description}
                            Manufacturer: ${drug.manufacturer}
                            Price: ${drug.price}
                            Stock: ${drug.stockQuantity}
                            Release Date: ${drug.releaseDate}
                            """.trimIndent()
                        }

                        runOnUiThread {
                            drugListTextView.text = drugsText
                        }

                        for (drug in drugs) {
                            Log.i("DrugActivity", "Drug: $drug")
                        }
                    } else {
                        Log.e("DrugActivity", "Response body is null")
                        runOnUiThread {
                            drugListTextView.text = "Response body is null"
                        }
                    }
                }
            }
        })
    }

    private fun getUnsafeOkHttpClient(): OkHttpClient {
        return try {
            val trustAllCerts = arrayOf<TrustManager>(object : X509TrustManager {
                override fun checkClientTrusted(chain: Array<java.security.cert.X509Certificate>, authType: String) {}
                override fun checkServerTrusted(chain: Array<java.security.cert.X509Certificate>, authType: String) {}
                override fun getAcceptedIssuers(): Array<java.security.cert.X509Certificate> = arrayOf()
            })

            val sslContext = SSLContext.getInstance("SSL")
            sslContext.init(null, trustAllCerts, java.security.SecureRandom())

            val sslSocketFactory = sslContext.socketFactory

            val builder = OkHttpClient.Builder()
            builder.sslSocketFactory(sslSocketFactory, trustAllCerts[0] as X509TrustManager)
            builder.hostnameVerifier { _, _ -> true }

            val logging = HttpLoggingInterceptor()
            logging.setLevel(HttpLoggingInterceptor.Level.BODY)
            builder.addInterceptor(logging)

            builder.build()
        } catch (e: Exception) {
            throw RuntimeException(e)
        }
    }
}
